namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;
    using DataFlow.Interfaces;

    public interface IAsyncCompletion<TResult,TData>
    {
        UniTask Complete(TResult value, TData data, ILifeTime lifeTime);
    }
    
    public interface IAsyncCommand
    {
        UniTask Execute();
    }
    
    public interface IAsyncCommand<T>
    {
        UniTask<T> Execute();
    }
    
    public interface IAsyncCommand<TValue,T>
    {
        UniTask<T> Execute(TValue value);
    }
    
    
}