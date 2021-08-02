namespace UniModules.Editor
{
    using System;
    using UniResourceSystem.Runtime.Interfaces;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
    public class ResourceHandle : IResource
    {
        [NonSerialized]
        protected Object _loadedItem;
        
        [SerializeField]
        public string assetName;

        [SerializeField]
        public string assetPath;
        
        [SerializeField]
        public string guid;

        [SerializeField]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
        [Sirenix.OdinInspector.PreviewField(Sirenix.OdinInspector.ObjectFieldAlignment.Center)]
#endif
        public Object asset;

        public string ItemName => assetName;

        public string Guid => guid;

        public string AssetPath => assetPath;

        public Object Asset => asset;
        
        #region public methods

        public bool HasValue()
        {
            return asset;
        }
        
        public T Load<T>()
            where T : class
        {
            T result = null;
            if (_loadedItem != null)
            {
                
                if (_loadedItem is T cached)
                    return cached;

                result = GetTargetFromSource<T>(_loadedItem);

            }
            else if (asset)
            {
                result = GetTargetFromSource<T>(asset);
            }
            else
            {
                result = LoadAsset<T>();
            }

            ApplyResource(result);

            return result;
        }
        
        public void Update(Object target)
        {
            this.asset = target;
            assetName = target.name;
            
            OnUpdateAsset(this.asset);
        }

        public ResourceHandle Update() {
            
            if (this.asset)
            {
                Update(this.asset);
                return this;
            }

            var target = Load<Object>();
            if(target)
            {
                Update(target);
            }

            return this;
        }

        #endregion

        protected T GetTargetFromSource<T>(Object source)
            where  T : class
        {
                        
            var result = source as T;
            if (result!=null)
            {
                return result;
            }

            if (source is GameObject gameObject)
            {
                result = gameObject.GetComponent<T>();
                if (result!=null)
                {
                    return result;
                }
            }

            return result;
            
        }
        
        private T ApplyResource<T>(T resource)
            where T : class
        {
            _loadedItem = resource as Object;
            return resource;
        }

        protected virtual TResult LoadAsset<TResult>()
            where TResult : class
        {
            return null;
        }

        protected virtual ResourceHandle OnUpdateAsset(Object targetAsset)
        {
            return this;
        }
        
    }
}
