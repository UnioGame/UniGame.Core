namespace UniModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces
{
    using UniGame.Core.Runtime.Interfaces;

    public interface IPoolItemsContainer<T> : IResetable where T : class
    {
        T Spawn();
        void Despawn(T item);
        void Reset();
    }
}