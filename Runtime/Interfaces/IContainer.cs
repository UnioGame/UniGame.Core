namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System.Collections.Generic;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IContainer<TData> : IPoolable
    {
        IReadOnlyList<TData> Items { get; }
    }
}