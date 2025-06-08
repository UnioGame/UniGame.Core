namespace UniGame.DataStructure.LinkedList.Interfaces
{
    using System;
    using global::UniGame.Core.Runtime;
    using global::UniGame.Core.Runtime.ObjectPool;

    public interface IListNode<T> : 
        IReadonlyValue<T>,
        IDisposable,
        IPoolable
    {
        IListNode<T> Previous { get; set; }
        IListNode<T> Next { get; set; }

    }
}