﻿using UniGame.Core.Runtime;
using UniGame.Core.Runtime.Extension;
using UnityEngine;

namespace UniGame.Runtime.ObjectPool
{
    public static class ObjectPool
    {

        public static ObjectPoolAsset activePool;

        public static ObjectPoolAsset PoolAsset => GetPool();

        public static ILifeTime AttachToLifeTime(Object poolAsset, ILifeTime lifeTime, bool createIfEmpty = false,int preload = 0)
        {
            return PoolAsset.AttachToLifeTime(poolAsset, lifeTime, createIfEmpty,preload);
        }

        public static bool HasCustomPoolLifeTimeFor(this Object poolAsset)
        {
            var originLifeTime = PoolAsset.LifeTime;
            var pool = PoolAsset.GetPool(poolAsset);
            return pool != null && pool.Owner != originLifeTime;
        }
        
        public static ILifeTime ApplyPoolAssetLifeTime(this ILifeTime lifeTime)
        {
            return PoolAsset.AttachToLifeTime(lifeTime);
        }
        
        // These methods allows you to spawn prefabs via Component with varying levels of transform data
        public static T Spawn<T>(Object asset) where T : Object
        {
            return PoolAsset.Spawn<T>(asset);
        }

        public static T Spawn<T>(GameObject prefab)
        {
            return PoolAsset.Spawn<T>(prefab);
        }
        

        public static  T Spawn<T>(Object target, Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false)
            where T : Object
        {
            return PoolAsset.Spawn<T>(target,position,rotation,parent,stayWorld);
        }

        public static  T Spawn<T>(Object target,bool activate, Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false)
            where T : Object
        {
            var asset = PoolAsset.Spawn<T>(target,position,rotation,parent,stayWorld);
            var gameObjectAsset = asset.GetRootAsset() as GameObject;
            gameObjectAsset?.SetActive(activate);
            return asset;
        }
        
        // These methods allows you to spawn prefabs via GameObject with varying levels of transform data
        public static GameObject Spawn(GameObject prefab)
        {
            return PoolAsset.Spawn(prefab);
        }

        public static  GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation,bool stayWorld = false)
        {
            return PoolAsset.Spawn(prefab,position,rotation,stayWorld);
        }

        public static  GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld)
        {
            return PoolAsset.Spawn(prefab,position,rotation,parent,stayWorld);
        }

        public static  Object Spawn(Object prefab, Vector3 position, Quaternion rotation, Transform parent,bool stayWorld, int preload)
        {
            return PoolAsset.Spawn(prefab,position,rotation,parent,stayWorld,preload);
        }

        public static  GameObject Spawn(GameObject prefab,bool activate, Vector3 position, Quaternion rotation, Transform parent = null,bool stayWorld = false,int preload = 0)
        {
            return PoolAsset.Spawn(prefab,activate,position,rotation,parent,stayWorld,preload);
        }
        
        public static void CreatePool(Object targetPrefab, int preloads = 0)
        {
            PoolAsset.CreatePool(targetPrefab,preloads);
        }

        public static  void DestroyPool(Object poolAsset)
        {
            PoolAsset.DestroyPool(poolAsset);
        }
        
        // This allows you to despawn a clone via GameObject, with optional delay
        public static  void Despawn(Object clone,bool destroy = false)
        {
            PoolAsset.Despawn(clone,destroy);
        }

        public static  void RemoveFromPool(GameObject target)
        {
            PoolAsset.RemoveFromPool(target);
        }

        private static ObjectPoolAsset GetPool()
        {
            if (activePool) return activePool;
            activePool = new GameObject(nameof(ObjectPoolAsset)).AddComponent<ObjectPoolAsset>();
            return activePool;
        }
        
    }
}