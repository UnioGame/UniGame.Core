namespace UniModules.UniGame.Core.Runtime.Rx
{
    using System.Collections.Generic;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IUniLinkedList<T> :
        IPoolable,
        IEnumerable<ListNode<T>>
    {
        ListNode<T> Add(T value);
        void        Remove(ListNode<T> node);
        void        RemoveSince(ListNode<T> node);
    }
}