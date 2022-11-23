using UniGame.Core.Runtime.ObjectPool;

namespace UniGame.Core.Runtime.Rx
{
    using System;

    public interface IRecycleObservable<T> :  IObservable<T>,IPoolable
    {

    }
}
