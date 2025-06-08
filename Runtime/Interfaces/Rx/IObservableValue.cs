namespace UniGame.Runtime.Rx
{
    using System;
    using Core.Runtime;

    public interface IObservableValue<out T> : 
        IObservable<T>, 
        IDisposable,
        IReadonlyDataValue<T>, 
        IValueContainerStatus
    {
    }
}