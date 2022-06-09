namespace UniModules.UniGame.Core.Runtime.Interfaces.Rx
{
    using UniRx;

    public interface IReadonlyRecycleReactiveProperty<TValue> : 
        IReadOnlyReactiveProperty<TValue>,
        IRecycleObservable<TValue>, 
        IContainerValueStatus
    {
    }
}