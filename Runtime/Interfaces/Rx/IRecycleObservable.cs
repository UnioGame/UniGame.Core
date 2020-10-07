using System;

namespace UniModules.UniCore.Runtime.Interfaces.Rx
{
    using ObjectPool.Runtime.Interfaces;

    public interface IRecycleObservable<T> :  IObservable<T>,IPoolable
    {

    }
}
