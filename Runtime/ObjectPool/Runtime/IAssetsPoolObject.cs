using System;
using UniGame.Core.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGame.Runtime.ObjectPool
{
    public interface IAssetsPoolObject : ILifeTimeContext, IDisposable
    {
        Object FastSpawn(Vector3 position, Quaternion rotation, Transform parent = null, bool stayWorld = false);
        void FastDespawn(Object clone, bool destroy = false);
        void PreloadAsset();
        void UpdatePreload();
    }
}