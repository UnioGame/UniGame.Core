namespace UniGame.Runtime.ObjectPool
{
    using System;
    using UniGame.Core.Runtime.ObjectPool;
    using UnityEngine;

    [Serializable]
    public class BasePoolItem : IPoolable
    {
        [SerializeField]
        public string typeName;
        [SerializeField]
        public int count;

        public virtual void Release()
        {
            
        }
    }
}