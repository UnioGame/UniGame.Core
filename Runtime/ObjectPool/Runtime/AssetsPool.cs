namespace UniModules.UniCore.Runtime.ObjectPool.Runtime
{
    using UniModules.UniGame.Core.Runtime.DataFlow.Extensions;
    using UnityEngine;

    public class AssetsPool : MonoBehaviour
    {
        [Tooltip("The prefab the clones will be based on")]
        public Object asset;

        [Tooltip("Should this pool preload some clones?")]
        public int preload;

        public bool ownPoolLifeTime = false;

        // Execute preloaded count
        protected virtual void Awake()
        {
            if (!asset) return;
            
            ObjectPool.CreatePool(asset,preload);

            if (ownPoolLifeTime)
                ObjectPool.AttachToLifeTime(asset, this.GetAssetLifeTime(), true);
        }

    }
}