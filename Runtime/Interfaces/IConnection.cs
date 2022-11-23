namespace UniGame.Core.Runtime
{
    using System;

    public interface IConnection<in T>
    {
        IDisposable Connect(T source);
    }
}