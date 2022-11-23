using UnityEngine;

namespace UniGame.Runtime.ObjectPool
{
    public static class ObjectPoolData
    {
        public static Transform _root;
        public static Transform RootContainer
        {
            get
            {
                if (_root) return _root;
                var asset = new GameObject(nameof(ObjectPoolData));
                Object.DontDestroyOnLoad(asset);
                _root = asset.transform;
                return _root;
            }
        }
    }
}