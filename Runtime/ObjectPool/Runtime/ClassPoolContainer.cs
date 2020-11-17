namespace UniModules.UniCore.Runtime.ObjectPool.Runtime
{
    using Interfaces;
    using UnityEngine;

    public class ClassPoolContainer : IPoolContainer
    {
        public bool Contains<T>() 
            where T : class
        {
            return ClassPoolItem<T>.Count > 0;
        }

        public T Pop<T>()
            where T : class
        {
            return ClassPoolItem<T>.Dequeue();
        }

        public void Push<T>(T item)
            where T : class
        {
            ClassPoolItem<T>.Enqueue(item);
        }
		
    }
}