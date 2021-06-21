using UniCore.Runtime.ProfilerTools;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniGame.Core.Runtime.Interfaces;

namespace UniModules.UniCore.Runtime.ObjectPool.Runtime
{
    using System;
    using System.Collections.Generic;
    using ProfilerTools;
    using Common;
    using Interfaces;
    using ProfilerTools;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UnityEngine;
    using Object = UnityEngine.Object;
    
    [Serializable]
    public class AssetsPoolObject
    {
        private LifeTimeDefinition _lifeTime;
        private GameObject _gameObjectAsset;
        private Component _componentAsset;
        private DisposableAction _disposableAction;
        
        private Func<Vector3, Quaternion, Transform, bool, Object> _factoryMethod;
        
        [Tooltip("The prefab the clones will be based on")]
        public Object asset;

        [Tooltip("Should this pool preload some clones?")]
        public int preload;

        private Transform containerObject;
        
        [Tooltip("Should this pool have a maximum amount of spawnable clones?")]
        public int capacity;

        // The total amount of created prefabs
        public int total;

        // All the currently cached prefab instances
        public Stack<Object> cache = new Stack<Object>();


        public ILifeTime LifeTime => _lifeTime;
        
        public AssetsPoolObject AttachLifeTime(ILifeTime lifeTime)
        {
            var disposableAction = ClassPool.Spawn<DisposableAction>();
            disposableAction.Initialize(Dispose);
            lifeTime.AddDispose(disposableAction);
            return this;
        }

        public AssetsPoolObject ResetLifeTime()
        {
            _disposableAction?.Complete();
            return this;
        }

        public void Dispose() =>  _lifeTime.Release();
        
        public void Initialize(Object objectAsset, int preloadCount = 0,Transform container = null)
        {
            _lifeTime ??= new LifeTimeDefinition();
            
            asset = objectAsset;
            preload = preloadCount;
            containerObject = container;
            
            switch (asset) {
                case GameObject gameObjectTarget:
                    _factoryMethod = CreateGameObject;
                    _gameObjectAsset = gameObjectTarget;
                    break;
                case Component componentTarget:
                    _componentAsset = componentTarget;
                    _gameObjectAsset = componentTarget.gameObject;
                    _factoryMethod = CreateGameObject;
                    break;
                default:
                    _factoryMethod = CreateAsset;
                    break;
            }

            _lifeTime.AddCleanUpAction(OnDisable);
            
            ObjectPoolAsset.AllPools.Add(this);
            UpdatePreload();
        }

        // This will return a clone from the cache, or create a new instance
        public Object FastSpawn(Vector3 position, Quaternion rotation, Transform parent = null, bool stayWorld = false)
        {
            if (!asset) {
                Debug.LogError("Attempting to spawn null");
                return null;
            }

            // Attempt to spawn from the cache
            while (cache.Count > 0) {
            
                var clone = cache.Pop();
                if (!clone) {
                    GameLog.LogErrorFormat("The {0} pool contained a null cache entry");
                    continue;
                }

                GameProfiler.BeginSample("ObjectPool.FastSpawn");

                ApplyGameAssetProperties(clone, position, rotation, parent, stayWorld);

                GameProfiler.EndSample();

                return clone;
            }

            // Make a new clone?
            if (capacity <= 0 || total < capacity) {
                var clone = FastClone(position, rotation, parent, stayWorld);
                return clone;
            }

            return null;
        }

        // This will despawn a clone and add it to the cache
        public void FastDespawn(Object clone, bool destroy = false)
        {
            if (!clone) return;
                        
            if (clone is IPoolable poolable)
            {
                poolable.Release();
            }

            if (destroy) {
                Object.Destroy(clone);
                return;
            }

            var target = OnObjectDespawn(clone);
            // Add it to the cache
            cache.Push(target);

        }

        // This allows you to make another clone and add it to the cache
        public void PreloadAsset()
        {
            if (!asset) return;
            // Create clone
            var clone = FastClone(Vector3.zero, Quaternion.identity, null);
            // Add it to the cache
            cache.Push(OnObjectDespawn(clone));
        }
        
        // Makes sure the right amount of prefabs have been preloaded
        public void UpdatePreload()
        {
            if (asset == null) return;
            for (var i = total; i < preload; i++) {
                PreloadAsset();
            }
        }

        private Object FastClone(Vector3 position, Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            if (!asset) return null;

            var clone = _factoryMethod(position, rotation, parent, stayWorldPosition);

            total += 1;

            return clone;
        }

        // Remove pool from list
        protected void OnDisable()
        {
            ObjectPoolAsset.AllPools.Remove(this);
            ObjectPoolAsset.AllSourceLinks.Remove(asset);
        }

        private Object CreateGameObject(
            Vector3 position,
            Quaternion rotation, 
            Transform parent = null, 
            bool stayWorldPosition = false)
        {
            if (!asset) return null;
            var result = Object.Instantiate(_gameObjectAsset, position, rotation);
            var resultTransform = result.transform;
            if (resultTransform.parent != parent)
                resultTransform.SetParent(parent, stayWorldPosition);
            return result;
        }

        private void ApplyGameAssetProperties(
            Object target, 
            Vector3 position,
            Quaternion rotation, 
            Transform parent, 
            bool stayWorldPosition = false)
        {
            switch (target) {
                case Component componentTarget:
                    ApplyGameAssetProperties(componentTarget.gameObject, position, rotation, parent, stayWorldPosition);
                    break;
                case GameObject gameObjectTarget:
                    ApplyGameAssetProperties(gameObjectTarget, position, rotation, parent, stayWorldPosition);
                    break;
            }
        }

        private void ApplyGameAssetProperties(GameObject target, Vector3 position,
            Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            var transform = target.transform;
            transform.localPosition = position;
            transform.localRotation = rotation;

            if (transform.parent != parent)
                transform.SetParent(parent, stayWorldPosition);

            // Hide it
            target.SetActive(false);
        }

        private GameObject ResetGameObjectState(GameObject targetGameObject)
        {
            if (!targetGameObject)
                return targetGameObject;
            
            targetGameObject.SetActive(false);
            // Move it under this GO
            if (targetGameObject.transform.parent != null) 
                targetGameObject.transform.SetParent(containerObject, false);

            return targetGameObject;
        }
        
        private Object OnObjectDespawn(Object asset)
        {
            switch (asset) {
                case Component componentTarget:
                    ResetGameObjectState(componentTarget.gameObject);
                    break;
                case GameObject gameObjectTarget:
                    ResetGameObjectState(gameObjectTarget);
                    break;
            }

            return asset;
        }
        
        private Object CreateAsset(Vector3 position,
            Quaternion rotation, Transform parent = null, bool stayWorldPosition = false)
        {
            return !asset ? null : Instantiate(asset);
        }
    }
}