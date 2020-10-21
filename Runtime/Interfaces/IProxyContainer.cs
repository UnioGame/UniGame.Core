namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System.Collections.Generic;

    public interface IProxyContainer<TSource, TTarget> : IContainer<TTarget> where TSource : class, TTarget
    {
        void AddRange(IReadOnlyList<TSource> sources);
        void Add(TTarget item);
    }
}