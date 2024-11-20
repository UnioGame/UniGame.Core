using UniModules.UniCore.Runtime.Common;
using UniModules.UniCore.Runtime.DataFlow;

namespace UniGame.Runtime.ObjectPool
{
    using System.Linq;
    using UniGame.Core.Runtime.ObjectPool;
    using Core.Runtime;
    using global::UniGame.Core.Runtime.Extension;
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    // This component allows you to pool Unity objects for fast instantiation and destruction
    [AddComponentMenu("UniGame/ObjectPool/Pool")]
    public class ObjectPoolAsset : MonoBehaviour, ILifeTimeContext
    {
        #region private fields

        private const string RootObjectName = "PoolsRootObject";
        
        private LifeTimeDefinition _lifeTime;
        private DisposableAction _disposableAction;
        private GameObject _poolsRoot;

        #endregion

        #region static data

        private const int DefaultBufferSuze = 128;
        
        // The reference between a spawned GameObject and its pool
        public static readonly Dictionary<Object, AssetsPoolObject> allCloneLinks = new(DefaultBufferSuze);
        
        //The reference between a spawned source GameObject and its pool
        public static Dictionary<Object, AssetsPoolObject> allSourceLinks = new(DefaultBufferSuze);

        private static void RemovePool(AssetsPoolObject poolObject)
        {
            allCloneLinks.RemoveWithValue(poolObject);
            allSourceLinks.Remove(poolObject.asset);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnReset()
        {
            allCloneLinks.Clear();
            allSourceLinks.Clear();
        }

        #endregion

        #region public properties

        public ILifeTime LifeTime => _lifeTime;
        
        #endregion
        
        public ILifeTime AttachToLifeTime(ILifeTime lifeTime)
        {
            //de-attach from current lifetime
            _disposableAction?.Complete();
            //bind new lifetime
            _disposableAction = ClassPool.Spawn<DisposableAction>();
            _disposableAction.Initialize(Destroy);
            
            lifeTime.AddDispose(_disposableAction);
            
            return lifeTime;
        }

        public ILifeTime AttachToLifeTime(Component poolAsset, ILifeTime lifeTime, bool createIfEmpty = false,
            int preload = 0)
        {
            return AttachToLifeTime(poolAsset.gameObject, lifeTime, createIfEmpty, preload);
        }

        public ILifeTime AttachToLifeTime(GameObject poolAsset, ILifeTime lifeTime, bool createIfEmpty = false, int preload = 0)
        {
            var pool = GetPool(poolAsset);
            pool ??= (createIfEmpty ? CreatePool(poolAsset,preload) : null);
            pool?.AttachLifeTime(lifeTime);
            
            return lifeTime;
        }
        
        // These methods allows you to spawn prefabs via Component with varying levels of transform data
        public T Spawn<T>(Object asset) where T : Object
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            return Spawn<T>(asset, Vector3.zero, Quaternion.identity, null,false) as T;
        }

        public T Spawn<T>(GameObject prefab)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return default;
#endif
            var item = Spawn(prefab, Vector3.zero, Quaternion.identity, null, false);
            var result = item.GetComponent<T>();
            if (result == null)
                Despawn(item);
            
            return result;
        }
        

        public T Spawn<T>(Object target, Vector3 position, 
            Quaternion rotation, Transform parent = null,
            bool stayWorld = false)
            where T : Object
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            var component = target as Component;
            var isComponent = component != null;
            var isGameObject = target is GameObject;

            if (!isComponent && !isGameObject) 
                return Instantiate(target) as T;
            
            // Clone this prefabs's GameObject
            var asset = isComponent ? component.gameObject : target;
            var clone = Spawn(asset, position, rotation, parent, stayWorld, 0);

            var result = isComponent && clone is GameObject gameAsset 
                ? gameAsset.GetComponent<T>() 
                : clone;
            
            // Return the same component from the clone
            return result as T;
        }

        // These methods allows you to spawn prefabs via GameObject with varying levels of transform data
        public GameObject Spawn(GameObject prefab)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null,false, 0) as GameObject;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation,bool stayWorld = false)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            return Spawn(prefab, position, rotation, null, stayWorld, 0) as GameObject;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position,
            Quaternion rotation, Transform parent,bool stayWorld)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            return Spawn(prefab, position, rotation, parent, stayWorld, 0) as GameObject;
        }

        public GameObject Spawn(GameObject prefab,bool activate,
            Vector3 position, Quaternion rotation, Transform parent,bool stayWorld, int preload)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            var pawn = Spawn(prefab, position, rotation, parent, stayWorld, preload) as GameObject;
            pawn?.SetActive(activate);
            return pawn;
        }
        
        public GameObject Spawn(GameObject prefab,bool activate, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            var pawn = Spawn(prefab, position, rotation, parent, stayWorld, 0) as GameObject;
            pawn?.SetActive(activate);
            return pawn;
        }
        
        public Object Spawn(
            Object source, 
            Vector3 position, 
            Quaternion rotation, 
            Transform parent,
            bool stayWorld, int preload)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            
#if UNITY_EDITOR
            if (source == null)
            {
                Debug.LogError("Attempting to spawn a null prefab");
                return default;
            }
#endif
            var objectAsset = source as GameObject;
            var component = source as Component;
            
            if(objectAsset == null && component == null)
                return Instantiate(source);
            
            var prefab = objectAsset !=null 
                ? objectAsset 
                : component.gameObject;
            
            var pool = CreatePool(prefab, preload);
            // Spawn a clone from this pool
            var clone = pool.Spawn(position, rotation, parent,stayWorld);

            allCloneLinks[clone] = pool;
            
            return clone;
        }
        
        public async UniTask<T> SpawnAsync<T>(GameObject prefab,CancellationToken token = default)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return default;
#endif
            var item = await SpawnAsync(prefab, Vector3.zero,
                Quaternion.identity, null, false,token:token);
            var result = item.GetComponent<T>();
            if (result == null) Despawn(item);
            return result;
        }

        public async UniTask<T> SpawnAsync<T>(
            Object source,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            bool stayWorld,
            int preload = 0,
            CancellationToken token = default)
        {
            var pawn = await SpawnAsync(source, position, rotation, parent, stayWorld, preload,token);
            return pawn switch
            {
                T result => result,
                GameObject gameObject => gameObject.GetComponent<T>(),
                _ => default
            };
        }

        public async UniTask<Object> SpawnAsync(
            Object source, 
            Vector3 position, 
            Quaternion rotation,
            Transform parent,
            bool stayWorld, 
            int preload = 0,
            CancellationToken token = default)
        {
            
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            
#if UNITY_EDITOR
            if (source == null)
            {
                Debug.LogError("Attempting to spawn a null prefab");
                return default;
            }
#endif
            var objectAsset = source as GameObject;
            var component = source as Component;
            
            if(objectAsset == null && component == null)
                return Instantiate(source);
            
            var prefab = objectAsset !=null 
                ? objectAsset 
                : component.gameObject;
            
            var pool = CreatePool(prefab);
            var spawnAmount = preload + 1;
            var isSingle = spawnAmount == 1;
            
            // Spawn a clone from this pool
            var clone = await pool
                .SpawnAsync(spawnAmount,position, rotation, parent,stayWorld,token);
            
            if (isSingle) return clone.First;

            var clones = clone.Items;
            for (var i = 1; i < clones.Length; i++)
            {
                var o = clones[i];
                allCloneLinks[o] = pool;
                Despawn(o);
            }
            
            var first =  clone.First;
            allCloneLinks[first] = pool;
            return first;
        }
        
        public async UniTask<GameObject> SpawnAsync(
            GameObject prefab, 
            Vector3 position,
            Quaternion rotation, 
            Transform parent,
            bool stayWorld,
            CancellationToken token = default)
        {
            var pawn = await SpawnAsync(prefab, position, rotation, parent, stayWorld, 0,token);
            return pawn as GameObject;
        }

        public AssetsPoolObject CreatePool(Component targetAsset, int preloads = 0)
        {
            return CreatePool(targetAsset.gameObject, preloads);
        }

        public AssetsPoolObject CreatePool(GameObject targetAsset, int preloads = 0)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
            if (!targetAsset) return null;

            // Find the pool that handles this prefab
            if (allSourceLinks.TryGetValue(targetAsset, out var pool))
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
            var container = new GameObject(targetAsset.name);
            DontDestroyOnLoad(container);
            var containerTransform = container.transform;
            containerTransform.SetParent(ObjectPoolData.RootContainer.transform);
            
            pool = new AssetsPoolObject();
            allSourceLinks.Add(targetAsset, pool);
            
            containerTransform.SetParent(_poolsRoot.transform,false);
            pool.Initialize(targetAsset,LifeTime,preloads,containerTransform);
            pool.LifeTime.AddCleanUpAction(() => RemovePool(pool));

            return pool;
            
        }

        public AssetsPoolObject GetPool(Object poolAsset)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return null;
#endif
#if UNITY_EDITOR
            if (!poolAsset)
            {
                Debug.LogError($"EMPTY {nameof(poolAsset)}",this);
                return null;
            }
#endif
            
            if (allSourceLinks.TryGetValue(poolAsset, out var pool))
                return pool;
            
            return allCloneLinks.GetValueOrDefault(poolAsset);
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
        public void Despawn(Object asset,bool destroy = false)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return;
#endif
            if (!asset) return;

            var clone = asset.GetRootAsset();
            
            // Try and find the pool associated with this clone
            if (!allCloneLinks.TryGetValue(clone, out var pool))
            {
                Destroy(clone);
                return;
            }
            
            if (asset is IPoolable poolable)
                poolable.Release();

            // Remove the association
            allCloneLinks.Remove(clone);
            // Despawn it
            pool.Despawn(clone, destroy);
        }

        public void RemoveFromPool(GameObject target)
        {
            if (!allCloneLinks.TryGetValue(target, out var pool)) return;
            // Remove the association
            allCloneLinks.Remove(target);
        }
        
        public async UniTask<ObjectsItemResult<GameObject>> SpawnAsync(
            Component source, 
            int count,
            Vector3 position, 
            Quaternion rotation,
            Transform parent,
            bool stayWorld, 
            CancellationToken token = default)
        {
            return await SpawnAsync(source.gameObject,count,position,rotation,parent,stayWorld,token);
        }

        #region private methods

                
        public async UniTask<ObjectsItemResult<GameObject>> SpawnAsync(
            GameObject source, 
            int count,
            Vector3 position, 
            Quaternion rotation,
            Transform parent,
            bool stayWorld, 
            CancellationToken token = default)
        {
            
#if UNITY_EDITOR
            if (Application.isPlaying == false) return ObjectsItemResult<GameObject>.Empty;
#endif
            
#if UNITY_EDITOR
            if (!source)
            {
                Debug.LogError("Attempting to spawn a null prefab");
                return ObjectsItemResult<GameObject>.Empty;
            }
#endif
            var pool = CreatePool(source,count);
            // Spawn a clone from this pool
            
            var clones = await pool
                .SpawnAsync(count,position, rotation, parent,stayWorld,token);
            
            return clones;
        }
        
        private void Destroy()
        {
            if(_poolsRoot)
                Destroy(_poolsRoot);
            Destroy(gameObject);
        }
        
        private void Awake()
        {
            _lifeTime = new LifeTimeDefinition();
            _lifeTime.AddCleanUpAction(OnDestroyAction);
        }

        private void OnDestroy() => _lifeTime?.Terminate();

        private void OnDestroyAction()
        {
            _disposableAction?.Complete();
            
            var myPools = allSourceLinks
                .Where(x => x.Value.Owner == LifeTime)
                .Select(x => x.Value)
                .ToList();
            
            foreach (var poolObjectValue in myPools)
            {
                poolObjectValue.Dispose();
            }
        }
        
        private static bool ClearCollectionPredicate(Object asset, AssetsPoolObject poolObject) => !asset;
        
        #endregion
        
    }
}