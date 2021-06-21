namespace UniModules.UniCore.Runtime.ObjectPool.Runtime
{
    using System.Collections.Generic;
    using UnityEngine;

    // This component allows you to pool Unity objects for fast instantiation and destruction
    [AddComponentMenu("UniGame/ObjectPool/Pool")]
    public class ObjectPoolAsset : MonoBehaviour
    {
        private static GameObject poolsRoot;

        // All the currently active pools in the scene
        public static List<AssetsPoolObject> AllPools = new List<AssetsPoolObject>();
        
        // The reference between a spawned GameObject and its pool
        //public static Dictionary<Object, AssetsPoolObject> AllLinks = new Dictionary<Object, AssetsPoolObject>();
        
        //The reference between a spawned source GameObject and its pool
        public static Dictionary<Object, AssetsPoolObject> AllSourceLinks = new Dictionary<Object, AssetsPoolObject>();

        // These methods allows you to spawn prefabs via Component with varying levels of transform data
        public static T Spawn<T>(Object asset)
            where T : Object
        {
            return Spawn<T>(asset, Vector3.zero, Quaternion.identity, null,false) as T;
        }

        public static T Spawn<T>(GameObject prefab)
        {
            var item = Spawn(prefab, Vector3.zero, Quaternion.identity, null, false);
            var result = item.GetComponent<T>();
            if (result == null)
            {
                Despawn(item);
            }
            return result;
        }
        

        public static T Spawn<T>(Object target, Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false)
            where T : Object
        {
            var isComponent = target is Component;
            // Clone this prefabs's GameObject
            var asset = target ? isComponent ? 
                    ((Component)target).gameObject : target : target;
            
            var clone = Spawn(asset, position, rotation, parent, stayWorld, 0);

            if (isComponent && clone is GameObject gameAsset) {
                return gameAsset.GetComponent<T>();
            }
            
            // Return the same component from the clone
            return clone as T;
        }

        // These methods allows you to spawn prefabs via GameObject with varying levels of transform data
        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null,false, 0) as GameObject;
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation,bool stayWorld = false)
        {
            return Spawn(prefab, position, rotation, null, stayWorld, 0) as GameObject;
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld)
        {
            return Spawn(prefab, position, rotation, parent, stayWorld, 0) as GameObject;
        }

        public static Object Spawn(Object prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld, int preload)
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
            // Was a clone created?
            // NOTE: This will be null if the pool's capacity has been reached
            return clone;
        }


        public static AssetsPoolObject CreatePool(Object targetPrefab, int preloads = 0)
        {
            if (!targetPrefab) return null;

            // Find the pool that handles this prefab
            if (AllSourceLinks.TryGetValue(targetPrefab, out var pool))
            {
                var preloadCount = preloads - pool.cache.Count;
                for (var i = 0; i < preloadCount; i++)
                {
                    pool.PreloadAsset();
                }
                return pool;
            }
            
            //create root
            if (!poolsRoot)
            {
                poolsRoot = new GameObject("PoolsRootObject");
            }

            // Create a new pool for this prefab?
            var container = new GameObject(targetPrefab.name);
            var containerTransform = container.transform;
            pool = new AssetsPoolObject();
            containerTransform.SetParent(poolsRoot.transform,false);
            pool.Initialize(targetPrefab,preloads,containerTransform);

            AllSourceLinks.Add(targetPrefab, pool);
            
            return pool;
            
        }

        public static void DestroyPool(Object poolAsset)
        {
            if (!poolAsset) return;

            // Try and find the pool associated with this clone
            if (!AllSourceLinks.TryGetValue(poolAsset, out var pool)) return;
            
            // Remove the association
            AllSourceLinks.Remove(poolAsset);
            // Despawn it
            pool.Dispose();
        }
        
        // This allows you to despawn a clone via GameObject, with optional delay
        public static void Despawn(Object clone,bool destroy = false)
        {
            if (!clone) return;

            // Try and find the pool associated with this clone
            if (AllLinks.TryGetValue(clone, out var pool))
            {
                // Remove the association
                AllLinks.Remove(clone);
                // Despawn it
                pool.FastDespawn(clone,destroy);
                return;
            }
            
            if(destroy)
            {
                // Fall back to normal destroying
                Destroy(clone);
                return;
            }

            switch (clone) {
                case GameObject gameObjectAsset:
                    gameObjectAsset.SetActive(false);
                    break;
                case Component componentAsset:
                    componentAsset.gameObject.SetActive(false);
                    break;
            }
        }


        public static void RemoveFromPool(GameObject target)
        {
            if (!AllLinks.TryGetValue(target, out var pool)) return;
            // Remove the association
            AllLinks.Remove(target);
        }

    }
}