namespace UniGame.Core.Runtime
{
    using System;

    public interface IReadOnlyData
    {
        object Get(Type type);
        TData Get<TData>();
        bool Contains<TData>();
    }
}