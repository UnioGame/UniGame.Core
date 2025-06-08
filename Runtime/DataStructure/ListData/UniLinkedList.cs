namespace UniGame.DataStructure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Runtime.Rx;
    using global::UniGame.Runtime.ObjectPool;

    /// <summary>
    /// Lightweight property broker.
    /// </summary>
    [Serializable]
    public class UniLinkedList<T> : IUniLinkedList<T>
    {
        public ListNode<T> root;
        public ListNode<T> last;

        public ListNode<T> Add(T value)
        {
            var node = ClassPool.Spawn<ListNode<T>>();
            
            // subscribe node, node as subscription.
            var next = node.SetValue(value);
            if (root == null)
            {
                root = last = next;
            }
            else
            {
                last.Next     = next;
                next.Previous = last;
                last = next;
            }
            
            return next;
        }

        public void RemoveSince(ListNode<T> node)
        {
            if (node == null)
                return;
            
            var previous = node.Previous;

            if (previous != null)
            {
                previous.Next = null;
            }
            else
            {
                root = null;
            }
            last = previous;
            
            while (node != null)
            {
                var next = node.Next;
                node.Dispose();
                node = next;
            }
        }

        public void Remove(ListNode<T> node)
        {
            if (node == root)
            {
                root = node.Next;
            }
            if (node == last)
            {
                last = node.Previous;
            }

            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
        }

        public void Dispose()
        {
            Release();
        }

        public void Release()
        {
            RemoveSince(root);
        }

        public void Apply(Action<T> action)
        {
            if (action == null)
                return;
            
            var node = root;
            while (node != null)
            {
                var next = node.Next;
                action(node.Value);
                node = next;
            }
        }

        #region IEnumerable<T>
        
        public IEnumerator<ListNode<T>> GetEnumerator()
        {
            var node = root;
            while (node != null)
            {
                var next = node.Next;
                yield return node;
                node = next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        #endregion
    }
}
