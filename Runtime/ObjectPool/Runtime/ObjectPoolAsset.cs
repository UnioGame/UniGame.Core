using System.Linq;

namespace UniModules.UniCore.Runtime.ObjectPool.Runtime
{
    using DataFlow;
    using UniModules.UniCore.Runtime.Rx.Extensions;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.Extension;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UniRx;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;
    using UnityEngine;

    // This component allows you to pool Unity objects for fast instantiation and destruction
    [AddComponentMenu("UniGame/ObjectPool/Pool")]
    public class ObjectPoolAsset : MonoBehaviour, ILifeTimeContext
    {
        #region private fields

        private const string RootObjectName = "PoolsRootObject";
        
        private LifeTimeDefinition _lifeTime;
        
        private GameObject _poolsRoot;

        #endregion

        #region static data
        
        // The reference between a spawned GameObject and its pool
        public static readonly Dictionary<Object, AssetsPoolObject> allCloneLinks = new Dictionary<Object, AssetsPoolObject>(128);
        
        //The reference between a spawned source GameObject and its pool
        public static Dictionary<Object, AssetsPoolObject> allSourceLinks = new Dictionary<Object, AssetsPoolObject>(128);

        private static void RemovePool(AssetsPoolObject poolObject)
        {
            allCloneLinks.RemoveWithValue(poolObject);
            allSourceLinks.Remove(poolObject.asset);
        }

        #endregion

        #region public properties

        public ILifeTime LifeTime => _lifeTime;
        
        #endregion
        
        public ILifeTime AttachToLifeTime(Object poolAsset, ILifeTime lifeTime, bool createIfEmpty = false)
        {
            var pool = GetPool(poolAsset);
            pool = pool != null 
                ? pool :
                createIfEmpty ? CreatePool(poolAsset) : null;

            pool?.AttachLifeTime(lifeTime);
            return lifeTime;
        }
        
        // These methods allows you to spawn prefabs via Component with varying levels of transform data
        public T Spawn<T>(Object asset) where T : Object
        {
            return Spawn<T>(asset, Vector3.zero, Quaternion.identity, null,false) as T;
        }

        public T Spawn<T>(GameObject prefab)
        {
            var item = Spawn(prefab, Vector3.zero, Quaternion.identity, null, false);
            var result = item.GetComponent<T>();
            if (result == null)
            {
                Despawn(item);
            }
            return result;
        }
        

        public T Spawn<T>(Object target, Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false)
            where T : Object
        {
            var component = target as Component;
            var isComponent = component != null;
            
            // Clone this prefabs's GameObject
            var asset = isComponent ? component.gameObject : target;
            
            var clone = Spawn(asset, position, rotation, parent, stayWorld, 0);

            var result = isComponent && clone is GameObject gameAsset ? 
                gameAsset.GetComponent<T>() : clone;
            
            // Return the same component from the clone
            return result as T;
        }

        // These methods allows you to spawn prefabs via GameObject with varying levels of transform data
        public GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null,false, 0) as GameObject;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation,bool stayWorld = false)
        {
            return Spawn(prefab, position, rotation, null, stayWorld, 0) as GameObject;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld)
        {
            return Spawn(prefab, position, rotation, parent, stayWorld, 0) as GameObject;
        }

        public Object Spawn(Object prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld, int preload)
        {
#if UNITY_EDITOR
            if (!prefab)
            {
                Debug.LogError("Attempting to spawn a null prefab");
                return null;
            }
#endif
            var pool = CreatePool(prefab, preload);
            // Spawn a clone from this pool
            var clone = pool.FastSpawn(position, rotation, parent,stayWorld);

            allCloneLinks[clone] = pool;
            
            return clone;
        }
        
        public AssetsPoolObject CreatePool(Object targetPrefab, int preloads = 0)
        {
            if (!targetPrefab) return null;

            // Find the pool that handles this prefab
            if (allSourceLinks.TryGetValue(targetPrefab, out var pool))
            {
                var preloadCount = preloads - pool.Cache.Count;
                for (var i = 0; i < preloadCount; i++)
                {
                    pool.PreloadAsset();
                }
                return pool;
            }
            
            //create root
            if (!_poolsRoot)
            {
                _poolsRoot = new GameObject(RootObjectName);
            }

            // Create a new pool for this prefab?
            var container = new GameObject(targetPrefab.name);
            var containerTransform = container.transform;
            containerTransform.SetParent(ObjectPoolData.RootContainer.transform);
            
            pool = new AssetsPoolObject();
            allSourceLinks.Add(targetPrefab, pool);
            
            containerTransform.SetParent(_poolsRoot.transform,false);
            pool.Initialize(targetPrefab,LifeTime,preloads,containerTransform);
            pool.LifeTime.AddCleanUpAction(() => RemovePool(pool));

            return pool;
            
        }

        public AssetsPoolObject GetPool(Object poolAsset)
        {

#if UNITY_EDITOR
            if (!poolAsset)
            {
                Debug.LogError($"EMPTY {nameof(poolAsset)}",this);
                return null;
            }
#endif
            
            if (allSourceLinks.TryGetValue(poolAsset, out var pool))
                return pool;
            if (allCloneLinks.TryGetValue(poolAsset, out var poolLink))
                return poolLink;

            return null;
        }
        
        public void DestroyPool(Object poolAsset)
        {
            if (!poolAsset) return;

            // Try and find the pool associated with this clone
            if (!allSourceLinks.TryGetValue(poolAsset, out var pool)) return;
            
            // Despawn it
            pool.Dispose();
        }
        
        // This allows you to despawn a clone via GameObject, with optional delay
        public void Despawn(Object clone,bool destroy = false)
        {
            if (!clone) return;

            // Try and find the pool associated with this clone
            if (allCloneLinks.TryGetValue(clone, out var pool))
            {
                // Remove the association
                allCloneLinks.Remove(clone);
                // Despawn it
                pool.FastDespawn(clone,destroy);
                return;
            }
            
            //despawn without pool
            Destroy(clone);
        }

        public void RemoveFromPool(GameObject target)
        {
            if (!allCloneLinks.TryGetValue(target, out var pool)) return;
            // Remove the association
            allCloneLinks.Remove(target);
        }

        #region private methods

        private void Awake()
        {
            _lifeTime = new LifeTimeDefinition();
            
            Observable.FromEvent(x => SceneManager.sceneLoaded += OnSceneLoaded,
                x => SceneManager.sceneLoaded -= OnSceneLoaded)
                .Subscribe()
                .AddTo(LifeTime);

            _lifeTime.AddCleanUpAction(OnDestroyAction);
        }

        private void OnDestroy() => _lifeTime.Terminate();

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnCleanUp();
        }
        
        private void OnCleanUp()
        {
            allCloneLinks.RemoveAll(ClearCollectionPredicate);
        }

        private void OnDestroyAction()
        {
            var myPools = allSourceLinks
                .Where(x => x.Value.Owner == LifeTime)
                .Select(x => x.Value)
                .ToList();
            
            foreach (var poolObjectValue in myPools)
            {
                poolObjectValue.Dispose();
            }
        }
        
        private static bool ClearCollectionPredicate(Object asset, AssetsPoolObject poolObject)
        {
            return !asset;
        }
        
        #endregion
        
    }
}