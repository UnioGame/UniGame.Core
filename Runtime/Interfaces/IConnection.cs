namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;

    public interface IConnection<T>
    {
        IDisposable Connect(T source);
    }
}