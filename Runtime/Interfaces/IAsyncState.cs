namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using UniRx;

    public interface IAsyncState : IAsyncState<Unit>
    {
    }

    public interface IAsyncState<TValue,TResult> : 
        IAsyncCommand<TValue,TResult>, 
        IAsyncEndPoint,
        ILifeTimeContext,
        IActiveStatus
    {
    }

    public interface IAsyncState<T> : IAsyncState<T, Unit> {
        
    }
}