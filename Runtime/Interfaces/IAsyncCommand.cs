namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;

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