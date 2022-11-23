namespace UniModules.UniGame.Core.Runtime.Rx
{
    using global::UniGame.Runtime.ObjectPool.Extensions;
    using global::UniGame.Core.Runtime.ObjectPool;

    public class ListNode<T> : IPoolable
    {
        public T Value;
        public ListNode<T> Previous;
        public ListNode<T> Next;
        
        public ListNode<T> SetValue(T target)
        {
            Value = target;
            return this;
        }

        public void Dispose() => this.Despawn();

        public void Release()
        {
            Value    = default;
            Previous = null;
            Next     = null;
        }

    }
}