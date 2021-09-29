namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;
    using DataFlow.Interfaces;

    public interface IAsyncCompletion<TResult,TData>
    {
        UniTask CompleteAsync(TResult value, TData data, ILifeTime lifeTime);
    }
    
    public interface IAsyncCommand
    {
        UniTask ExecuteAsync();
    }
    
    public interface IAsyncCommand<T>
    {
        UniTask<T> ExecuteAsync();
    }
    
    public interface IAsyncCommand<TValue,T>
    {
        UniTask<T> ExecuteAsync(TValue value);
    }
    
}