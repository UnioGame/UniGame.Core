namespace UniGame.Runtime.Rx
{
    using System;

    public interface IObservableFactory<T>
    {

        IObservable<T> Create();

    }
}
