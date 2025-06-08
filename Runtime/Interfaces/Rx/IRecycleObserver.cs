using UniGame.Core.Runtime.ObjectPool;

namespace UniGame.Runtime.Rx
{
    using System;

    public interface IRecycleObserver<T> : 
        IObserver<T>, 
        IPoolable,
        IDespawnable
    {
        
    }
}
