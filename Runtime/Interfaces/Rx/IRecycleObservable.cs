using UniGame.Core.Runtime.ObjectPool;

namespace UniGame.Runtime.Rx
{
    using System;

    public interface IRecycleObservable<T> :  IObservable<T>,IPoolable
    {

    }
}
