namespace UniModules.UniGame.Core.Runtime.Interfaces.Rx
{
    using System;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IRecycleObserver<T> : 
        IObserver<T>, 
        IPoolable,
        IDespawnable
    {
        
    }
}
