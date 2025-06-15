namespace UniGame.Core.Runtime.ObjectPool
{
    using global::UniGame.Core.Runtime;

    public interface IPoolItemsContainer<T> : IResettable where T : class
    {
        T Spawn();
        void Despawn(T item);
        void Reset();
    }
}