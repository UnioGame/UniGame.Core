using System;
using UniModules.UniGame.Core.Runtime.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModules.UniCore.Runtime.ObjectPool.Runtime
{
    public interface IAssetsPoolObject : ILifeTimeContext, IDisposable
    {
        Object FastSpawn(Vector3 position, Quaternion rotation, Transform parent = null, bool stayWorld = false);
        void FastDespawn(Object clone, bool destroy = false);
        void PreloadAsset();
        void UpdatePreload();
    }
}