namespace UniModules.UniGame.Core.Runtime.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::UniGame.Core.Runtime.ObjectPool;
    using UnityEditor;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public class VariantValue<TValue,TAsset,TApi> : 
        IVariantValue<TApi>,
        IPoolable
    {
        
        [SerializeReference]
        #if ODIN_INSPECTOR
        [InlineProperty]
        [HideLabel]
        [HideIf(nameof(IsUnityCommandInitialized))]
        #endif
        public TValue value;

        [Space]
#if  ODIN_INSPECTOR
        [InlineEditor]
        [ValueDropdown(nameof(GetAssets))]
        [HideIf(nameof(IsSerializedCommandInitialized))]
        [HideLabel]
#endif
        [SerializeField]
        public TAsset assetValue;
        
        public bool IsUnityCommandInitialized => assetValue != null;

        public bool IsSerializedCommandInitialized => value != null;

        public bool HasValue => value != null || assetValue != null;
        
        public TApi Value => Convert(value, assetValue);

        public void Release()
        {
            value      = default;
            assetValue = default;
        }
        
        public void SetValue(TApi variant)
        {
            switch (variant)
            {
                case TValue regularVariant:
                    value      = regularVariant;
                    assetValue = default;
                    break;
                case TAsset assetVariant :
                    value      = default;
                    assetValue = assetVariant;
                    break;
            }
        }

        protected virtual TApi Convert(TValue valueParameter, TAsset assetParameter)
        {
            if (valueParameter is TApi valueResult) return valueResult;
            if (assetParameter is TApi assetResult) return assetResult;
            return default;
        }
        
        protected virtual IEnumerable<TAsset> GetAssets()
        {
#if UNITY_EDITOR
            return AssetDatabase.FindAssets($"t:{typeof(TAsset).Name}").OfType<TAsset>();
#endif
            return Enumerable.Empty<TAsset>();
        }
        
    }
}
