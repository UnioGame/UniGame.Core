namespace UniGame.Core.Runtime
{
    using UniGame.Core.Runtime.ObjectPool;
    using System.Collections.Generic;

    public interface IContainer<TData> : IPoolable
    {
        IReadOnlyList<TData> Items { get; }
    }
}