using UnityEngine;

namespace UniModules.UniGame.Core.Runtime.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class VariantValue<TValue,TAsset,TApi> : IVariantValue<TApi>
    {
        
        [SerializeReference]
        #if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
        #endif
        public TValue value;

        [Space]
#if  ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
        [Sirenix.OdinInspector.ValueDropdown(nameof(GetAssets))]
        [Sirenix.OdinInspector.HideIf(nameof(IsSerializedCommandInitialized))]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        public TAsset assetValue;

                
        public bool IsUnityCommandInitialized => assetValue != null;

        public bool IsSerializedCommandInitialized => value != null;

        public bool HasValue => value != null || assetValue != null;
        
        public TApi Value => Convert(value, assetValue);

        protected virtual TApi Convert(TValue valueParameter, TAsset assetParameter)
        {
            if (valueParameter is TApi valueResult) return valueResult;
            if (assetParameter is TApi assetResult) return assetResult;
            return default;
        }
        
        protected virtual IEnumerable<TAsset> GetAssets()
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.FindAssets($"t:{typeof(TAsset).Name}").OfType<TAsset>();
#endif
        }
        
    }
}
