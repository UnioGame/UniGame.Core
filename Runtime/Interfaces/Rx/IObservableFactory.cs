namespace UniGame.Core.Runtime.Rx
{
    using System;

    public interface IObservableFactory<T>
    {

        IObservable<T> Create();

    }
}
