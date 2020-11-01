namespace UniModules.UniGame.Core.Runtime.Interfaces {
    using Cysharp.Threading.Tasks;

    public interface IAsyncRollback {
        UniTask Rollback();
    }
    
    public interface IAsyncRollback<TSource>
    {
        UniTask Rollback(TSource source);
    }
    
    public interface IAsyncRollback<TSource,TResult>
    {
        UniTask<TResult> Rollback(TSource source);
    }
    
}