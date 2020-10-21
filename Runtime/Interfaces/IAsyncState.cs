namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using UniRx;

    public interface IAsyncState : IAsyncState<Unit>
    {
    }

    public interface IAsyncState<T,TValue> : 
        IAsyncCommand<T,TValue>, 
        IAsyncEndPoint,
        ILifeTimeContext,
        IActiveStatus
    {
    }
    
    public interface IAsyncState<T> : 
        IAsyncCommand<T>, 
        IAsyncEndPoint,
        ILifeTimeContext,
        IActiveStatus
    {
    }
}