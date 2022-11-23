namespace UniGame.Core.Runtime.ObjectPool
{
    using global::UniGame.Core.Runtime;

    public interface IPoolItemsContainer<T> : IResetable where T : class
    {
        T Spawn();
        void Despawn(T item);
        void Reset();
    }
}