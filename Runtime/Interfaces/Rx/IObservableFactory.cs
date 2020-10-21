namespace UniModules.UniGame.Core.Runtime.Interfaces.Rx
{
    using System;

    public interface IObservableFactory<T>
    {

        IObservable<T> Create();

    }
}
