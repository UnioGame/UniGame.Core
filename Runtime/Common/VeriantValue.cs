using UnityEngine;

namespace UniModules.UniGame.Core.Runtime.Common {
    using System;

    [Serializable]
    public class VeriantValue<TValue,TAsset> {

        [SerializeReference]
        
        #if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
        #endif
        public TValue value;

        [SerializeField]
        public TAsset assetValue;

    }
}
