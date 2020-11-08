namespace UniModules.UniGame.Core.Runtime.Rx
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IUniLinkedList<T> :
        IPoolable
    {
        ListNode<T> Add(T value);
        void Remove(ListNode<T> node);
    }
}