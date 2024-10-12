﻿namespace UniGame.Runtime.ObjectPool
{
    using UniCore.Runtime.ProfilerTools;
    using UniGame.Core.Runtime.Extension;
    using UniModules.UniCore.Runtime.Common;
    using UniModules.UniCore.Runtime.DataFlow;
    using System;
    using System.Collections.Generic;
    using UniGame.Core.Runtime.ObjectPool;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using Object = UnityEngine.Object;
    
    [Serializable]
    public class AssetsPoolObject
    {
        #region private properties
        
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private GameObject _gameObjectAsset;
        private Component _componentAsset;
        private DisposableAction _disposableAction;
        private Func<Vector3, Quaternion, Transform, bool, Object> _factoryMethod;
        private Func<Vector3, Quaternion, Transform, bool, UniTask<Object>> _asyncFactoryMethod;
        
        #endregion

        public string sourceName;
        
        [Tooltip("The prefab the clones will be based on")]
        public Object asset;

        [Tooltip("Should this pool preload some clones?")]
        public int preload;

        public Transform containerObject;
        
        // The total amount of created prefabs
        public int total;

        // All the currently cached prefab instances
        public Stack<Object> Cache = new Stack<Object>();

        public ILifeTime Owner;

        public ILifeTime LifeTime => _lifeTime;

        public AssetsPoolObject AttachLifeTime(ILifeTime lifeTime)
        {
            //de-attach from current lifetime
            ResetParentLifeTime();

            Owner = lifeTime;
            //bind new lifetime
            _disposableAction = ClassPool.Spawn<DisposableAction>();
            _disposableAction.Initialize(Dispose);
            Owner.AddDispose(_disposableAction);
            
            return this;
        }

        public AssetsPoolObject ResetParentLifeTime()
        {
            _disposableAction?.Complete();
            return this;
        }

        public void Dispose() =>  _lifeTime.Terminate();
        
        public AssetsPoolObject Initialize(Object objectAsset,ILifeTime lifeTime, int preloadCount = 0,Transform root = null)
        {
            _lifeTime ??= new LifeTimeDefinition();
            _lifeTime.Release();
            _lifeTime.AddCleanUpAction(OnDestroy);

            sourceName = objectAsset.name;
            asset = objectAsset;
            preload = preloadCount;
            containerObject = root;
            
            switch (asset) {
                case GameObject gameObjectTarget:
                    _factoryMethod = CreateGameObject;
                    _asyncFactoryMethod = CreateGameObjectAsync;
                    _gameObjectAsset = gameObjectTarget;
                    break;
                case Component componentTarget:
                    _componentAsset = componentTarget;
                    _gameObjectAsset = componentTarget.gameObject;
                    _factoryMethod = CreateGameObject;
                    _asyncFactoryMethod = CreateGameObjectAsync;
                    break;
                default:
                    _factoryMethod = CreateAsset;
                    _asyncFactoryMethod = CreateAssetAsync;
                    break;
            }

            UpdatePreload();
            AttachLifeTime(lifeTime);
            
            return this;
        }

        // This will return a clone from the cache, or create a new instance
        public Object FastSpawn(Vector3 position, Quaternion rotation,
            Transform parent = null, bool stayWorld = false)
        {
#if UNITY_EDITOR
            if (!asset) {
                Debug.LogError("Attempting to spawn null");
                return null;
            }
#endif

            // Attempt to spawn from the cache
            while (Cache.Count > 0) {
            
                var clone = Cache.Pop();
                if (!clone) {
                    GameLog.LogWarningFormat("The {0} pool contained a null cache entry",sourceName);
                    continue;
                }

                clone = ApplyGameAssetProperties(clone, position, rotation, parent, stayWorld);
                return clone;
            }

            return FastClone(position, rotation, parent, stayWorld);;
        }
        
        
        // This will return a clone from the cache, or create a new instance
        public async UniTask<Object> FastSpawnAsync(Vector3 position, Quaternion rotation,
            Transform parent = null, bool stayWorld = false)
        {
#if UNITY_EDITOR
            if (!asset) {
                Debug.LogError("Attempting to spawn null");
                return null;
            }
#endif

            // Attempt to spawn from the cache
            while (Cache.Count > 0) {
            
                var clone = Cache.Pop();
                if (!clone) {
                    GameLog.LogWarningFormat("The {0} pool contained a null cache entry",sourceName);
                    continue;
                }

                clone = ApplyGameAssetProperties(clone, position, rotation, parent, stayWorld);
                return clone;
            }

            return await FastCloneAsync(position, rotation, parent, stayWorld);;
        }

        // This will despawn a clone and add it to the cache
        public void FastDespawn(Object clone, bool destroy = false)
        {
            if (!clone) return;
            
            if (clone is IPoolable poolable)
            {
                poolable.Release();
            }

            var target = clone.GetRootAsset();
            
            if (destroy) {
                Object.Destroy(target);
                return;
            }

            OnObjectDespawn(clone);
            // Add it to the cache
            Cache.Push(target);
        }
        

        // This allows you to make another clone and add it to the cache
        public void PreloadAsset()
        {
            if (!asset) return;
            // Create clone
            var clone = FastClone(Vector3.zero, Quaternion.identity, null);

            var rootAsset = clone.GetRootAsset();
            if (rootAsset is GameObject gameObjectClone)
            {
                gameObjectClone.transform.SetParent(containerObject);
            }
            
            // Add it to the cache
            Cache.Push(OnObjectDespawn(clone));
        }

        // Makes sure the right amount of prefabs have been preloaded
        public void UpdatePreload()
        {
            if (asset == null) return;
            for (var i = total; i < preload; i++) {
                PreloadAsset();
            }
        }

        #region private methods
        
        private void OnDestroy()
        {
            _disposableAction?.Complete();
            
            Owner = null;

            foreach (var item in Cache)
            {
                if (!item) continue;
                
                switch (item)
                {
                    case GameObject gameObject when gameObject.transform != containerObject:
                        Object.Destroy(gameObject);
                        break;
                    case Component component when component.transform != containerObject:
                        Object.Destroy(component.gameObject);
                        break;
                    case { } assetItem when !(assetItem is Component) && !(assetItem is GameObject):
                        Object.Destroy(assetItem);
                        break;
                }
            }
            
            Cache.Clear();
            
            if(containerObject)
                Object.Destroy(containerObject.gameObject);
        }

        private async UniTask<Object> FastCloneAsync(Vector3 position, Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            if (!asset) return null;
            var clone = await _asyncFactoryMethod(position, rotation, parent, stayWorldPosition);
            total += 1;
            return clone;
        }
        
        private Object FastClone(Vector3 position, Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            if (!asset) return null;

            var clone = _factoryMethod(position, rotation, parent, stayWorldPosition);

            total += 1;

            return clone;
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
        
        private async UniTask<Object> CreateGameObjectAsync(
            Vector3 position,
            Quaternion rotation, 
            Transform parent = null, 
            bool stayWorldPosition = false)
        {
            if (!_gameObjectAsset) return null;

            var operation =  Object
                .InstantiateAsync(_gameObjectAsset,1,parent, position, rotation);
            
            await operation.ToUniTask();
            
            var resultItems = operation.Result;
            
            if(resultItems.Length == 0) return null;
            
            var result = resultItems[0];
            var resultTransform = result.transform;
            if (resultTransform.parent != parent)
                resultTransform.SetParent(parent, stayWorldPosition);
            return result;
        }

        private Object ApplyGameAssetProperties(
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
            return target;
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
            
#if UNITY_EDITOR
            if(Application.isPlaying == false) return targetGameObject;
#endif
            if (targetGameObject == null) return targetGameObject;
            
            var parent = containerObject != null ? containerObject : null;
            // Move it under this GO
            if (targetGameObject.transform.parent != null) 
                targetGameObject.transform.SetParent(parent, false);

            targetGameObject.SetActive(false);
            
            return targetGameObject;
        }
        
        private Object OnObjectDespawn(Object target)
        {
            switch (target) {
                case Component componentTarget:
                    ResetGameObjectState(componentTarget.gameObject);
                    break;
                case GameObject gameObjectTarget:
                    ResetGameObjectState(gameObjectTarget);
                    break;
            }
            return target;
        }
        
        private Object CreateAsset(Vector3 position, Quaternion rotation, Transform parent = null, bool stayWorldPosition = false)
        {
            return !asset ? null : Object.Instantiate(asset);
        }
        
        private async UniTask<Object> CreateAssetAsync(Vector3 position,
            Quaternion rotation, 
            Transform parent = null, 
            bool stayWorldPosition = false)
        {
            if (asset == null) return null;
            var operation =  Object.InstantiateAsync(asset,1);
            await operation.ToUniTask();
            var resultItems = operation.Result;
            return resultItems.Length == 0 ? default : resultItems[0];
        }
        
        
        #endregion
    }
}