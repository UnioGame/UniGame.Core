namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;

    public interface IConnection<in T>
    {
        IDisposable Connect(T source);
    }
}