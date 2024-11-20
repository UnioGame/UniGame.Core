namespace UniGame.Runtime.ObjectPool
{
    using UniCore.Runtime.ProfilerTools;
    using UniGame.Core.Runtime.Extension;
    using UniModules.UniCore.Runtime.Common;
    using UniModules.UniCore.Runtime.DataFlow;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UniGame.Core.Runtime.ObjectPool;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using Object = UnityEngine.Object;
    
    [Serializable]
    public class AssetsPoolObject
    {
        #region private properties
        
        private LifeTimeDefinition _lifeTime = new();
        private GameObject _gameObjectAsset;
        private DisposableAction _disposableAction;
        
        #endregion

        public string sourceName;
        
        [Tooltip("The prefab the clones will be based on")]
        public GameObject asset;

        [Tooltip("Should this pool preload some clones?")]
        public int preload;

        public Transform containerObject;
        
        // The total amount of created prefabs
        public int total;

        // All the currently cached prefab instances
        public Stack<GameObject> Cache = new();

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
        
        
        public AssetsPoolObject Initialize(Component objectAsset,ILifeTime lifeTime, int preloadCount = 0,Transform root = null)
        {
            return Initialize(objectAsset.gameObject,lifeTime,preloadCount,root);
        }

        public AssetsPoolObject Initialize(GameObject objectAsset,ILifeTime lifeTime, int preloadCount = 0,Transform root = null)
        {
            _lifeTime ??= new LifeTimeDefinition();
            _lifeTime.Release();
            _lifeTime.AddCleanUpAction(OnDestroy);

            sourceName = objectAsset.name;
            asset = objectAsset;
            preload = preloadCount;
            containerObject = root;
            
            _gameObjectAsset = objectAsset;

            UpdatePreload();
            AttachLifeTime(lifeTime);
            
            return this;
        }

        // This will return a clone from the cache, or create a new instance
        public Object Spawn(Vector3 position, 
            Quaternion rotation,
            Transform parent = null, 
            bool stayWorld = false)
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

            return CreateGameObject(position, rotation, parent, stayWorld);
        }
        

        // This will despawn a clone and add it to the cache
        public void Despawn(Object clone, bool destroy = false)
        {
            if (!clone) return;
            
            if (clone is IPoolable poolable)
            {
                poolable.Release();
            }

            var target = clone.GetRootAsset() as GameObject;
            
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
            if (!_gameObjectAsset) return;
            
            // Create clone
            var clone = CreateGameObject(Vector3.zero, Quaternion.identity, null);

            var rootAsset = clone.GetRootAsset();
            if (rootAsset is GameObject gameObjectClone)
            {
                gameObjectClone.transform.SetParent(containerObject);
            }
            
            // Add it to the cache
            var pawn = OnObjectDespawn(clone);
            
            Cache.Push(pawn);
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
        
        // This will return a clone from the cache, or create a new instance
        public async UniTask<ObjectsItemResult<GameObject>> SpawnAsync(
            int count,
            Vector3 position, 
            Quaternion rotation,
            Transform parent = null, 
            bool stayWorld = false,
            CancellationToken token = default)
        {
#if UNITY_EDITOR
            if (!asset) {
                Debug.LogError("Attempting to spawn null");
                return ObjectsItemResult<GameObject>.Empty;
            }
#endif

            var result = TakeFromCache(count, position, rotation, parent, stayWorld);
            if(result.Success) return result;

            var spawnedItems = await CreateGameObjectAsync(
                    count,
                    position, rotation,
                    parent, stayWorld,token).AttachExternalCancellation(token);

            return spawnedItems;
        }

        public ObjectsItemResult<GameObject> TakeFromCache(
            int count,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null,
            bool stayWorld = false)
        {
            if(count > Cache.Count) return ObjectsItemResult<GameObject>.Empty;
            
            var result = ObjectsItemResult<GameObject>.Single;
            var spawnedCount = 0;
            var isSingle = count == 1;
            result.Items = isSingle ? Array.Empty<GameObject>() 
                : ArrayPool<GameObject>.Shared.Rent(count);
            result.Success = true;
            
            // Attempt to spawn from the cache
            while (Cache.Count > 0 && spawnedCount < count) {
                
                var clone = Cache.Pop();
                
                if (clone == null) {
                    GameLog.Log($"The {sourceName} pool contained a null cache entry",Color.red);
                    continue;
                }
                
                clone = ApplyGameAssetProperties(clone, position, rotation, parent, stayWorld);

                if (isSingle)
                {
                    result.First = clone;
                    return result;
                }
                
                result.Items[spawnedCount] = clone;
                spawnedCount++;
            }
            
            result.Length = spawnedCount;
            return result;
        }
        
        private void OnDestroy()
        {
            _disposableAction?.Complete();
            
            Owner = null;

            foreach (var item in Cache)
            {
                if (item == null) continue;
#if UNITY_EDITOR
                if (Application.isPlaying == false)
                {
                    Object.DestroyImmediate(item);
                    continue;
                }
#endif
                Object.Destroy(item);
            }
            
            Cache.Clear();

            if (containerObject == null) return;
            
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                Object.DestroyImmediate(containerObject.gameObject);
                return;
            }
#endif
            Object.Destroy(containerObject.gameObject);
        }
        
        private GameObject CreateGameObject(
            Vector3 position,
            Quaternion rotation, 
            Transform parent = null, 
            bool stayWorldPosition = false)
        {
            if (!asset) return default;
            
            var result = Object.Instantiate(_gameObjectAsset, position, rotation);
            var resultTransform = result.transform;
            if (resultTransform.parent != parent)
                resultTransform.SetParent(parent, stayWorldPosition);
            
            total += 1;
            
            return result;
        }


        private GameObject ApplyGameAssetProperties(
            Object target, 
            Vector3 position,
            Quaternion rotation, 
            Transform parent, 
            bool stayWorldPosition = false)
        {
            switch (target) {
                case Component componentTarget:
                    return ApplyGameAssetProperties(componentTarget.gameObject, position, rotation, parent, stayWorldPosition);
                case GameObject gameObjectTarget:
                    return ApplyGameAssetProperties(gameObjectTarget, position, rotation, parent, stayWorldPosition);
            }
            return default;
        }
        
        private GameObject ApplyGameAssetProperties(GameObject target, Vector3 position,
            Quaternion rotation, Transform parent, bool stayWorldPosition = false)
        {
            var transform = target.transform;
            transform.localPosition = position;
            transform.localRotation = rotation;

            if (transform.parent != parent)
                transform.SetParent(parent, stayWorldPosition);

            // Hide it
            target.SetActive(false);

            return target;
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
        
        private GameObject OnObjectDespawn(Object target)
        {
            switch (target) {
                case Component componentTarget:
                    return ResetGameObjectState(componentTarget.gameObject);
                case GameObject gameObjectTarget:
                    return ResetGameObjectState(gameObjectTarget);
            }
            return null;
        }
        
        private Object CreateAsset(Vector3 position, Quaternion rotation, Transform parent = null, bool stayWorldPosition = false)
        {
            if(asset == null) return default;
            return Object.Instantiate(asset);
        }
        
        private async UniTask<ObjectsItemResult<Object>> CreateAssetAsync(
            int count,
            Vector3 position,
            Quaternion rotation, 
            Transform parent = null, 
            bool stayWorldPosition = false,
            CancellationToken token = default)
        {
            if(asset == null) return ObjectsItemResult<Object>.Empty;
            if(count <= 0) return ObjectsItemResult<Object>.Empty;
            
            var operation =  Object.InstantiateAsync(asset,count);

            CleanupResourceOnCancellation(operation, token);
            
            var assetResult = await operation
                .ToUniTask(cancellationToken:token)
                .SuppressCancellationThrow();

            if (assetResult.IsCanceled)
            {
                operation.Cancel();
                throw new TaskCanceledException();
            }
            
            var resultItems = operation.Result;
            if (resultItems == null || resultItems.Length == 0)
                return ObjectsItemResult<Object>.Empty;
            
            return new ObjectsItemResult<Object>()
            {
                First = resultItems[0],
                Items = resultItems,
                Success = true,
            };
            
        }
                
        private async UniTask<ObjectsItemResult<GameObject>> CreateGameObjectAsync(
            int count,
            Vector3 position,
            Quaternion rotation, 
            Transform parent = null, 
            bool stayWorldPosition = false,
            CancellationToken token = default)
        {
            if (_gameObjectAsset == null) 
                return ObjectsItemResult<GameObject>.Empty;
            
            if(count <= 0) 
                return ObjectsItemResult<GameObject>.Empty;

            var operation =  Object.InstantiateAsync(_gameObjectAsset,count,parent, position, rotation,token);

#if UNITY_EDITOR
            CleanupResourceOnCancellation(operation, token);
#endif
            
            var assetResult = await operation
                .ToUniTask(cancellationToken:token)
                .SuppressCancellationThrow();

            if (assetResult.IsCanceled)
            {
                operation.Cancel();
                throw new TaskCanceledException();
            }
            
            var resultItems = operation.Result;
            
            if (resultItems == null || resultItems.Length == 0)
                return ObjectsItemResult<GameObject>.Empty;
            
            var spawnedCount = resultItems.Length;
            total += spawnedCount;
            
            return new ObjectsItemResult<GameObject>()
            {
                First = resultItems[0],
                Items = resultItems,
                Success = true,
                Length = spawnedCount,
            };
        }
        
        
        private void CleanupResourceOnCancellation<TAsset>(AsyncInstantiateOperation<TAsset> operation, CancellationToken token)
            where TAsset : Object
        {
#if UNITY_EDITOR
            operation.completed += operationHandle =>
            {
                if(!token.IsCancellationRequested) return;
                if(operation.Result == null) return;
                foreach (var gameObject in operation.Result)
                {
                    if(gameObject == null) continue;
                    Object.DestroyImmediate(gameObject);
                }
            };
#endif
        }
        #endregion
    }
}