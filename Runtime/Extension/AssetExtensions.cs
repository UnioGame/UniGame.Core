using UniModules.UniCore.Runtime.Utils;
using UniGame.Core.Runtime;
using UnityEngine;

namespace UniGame.Core.Runtime.Extension
{
    public static class AssetExtensions
    {
        private static MemorizeItem<Object, Object> _sharedItemsMap = 
            MemorizeTool.Memorize<Object,Object>(GetSharedAsset);

        public static TAsset ToSharedInstance<TAsset>(this TAsset asset, ILifeTime lifeTime)
            where TAsset : class
        {
            var instance = asset.ToSharedInstance();
            if (instance is Object assetInstance)
                assetInstance.DestroyWith(lifeTime);
            return instance;
        }

        public static TAsset ToSharedInstance<TAsset>(this TAsset asset) 
            where TAsset : class
        {
            if (asset is not ScriptableObject objectAsset) 
                return asset;
            var sharedAsset = _sharedItemsMap[objectAsset];
            if (sharedAsset == null)
            {
                _sharedItemsMap.Remove(objectAsset);
                sharedAsset = _sharedItemsMap[objectAsset];
            }

            return sharedAsset as TAsset;
        }

        private static Object GetSharedAsset(Object shared)
        {
            return Object.Instantiate(shared);
        }
    }
}