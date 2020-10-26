namespace UniModules.UniGame.Core.Runtime.Interfaces {
    using Cysharp.Threading.Tasks;

    public interface IAsyncRollback {
        UniTask Rollback();
    }
    
    public interface IAsyncRollback<TResult>
    {
        UniTask<TResult> Rollback();
    }
}