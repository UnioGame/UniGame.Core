namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    public interface IAsyncRollbackCommand : IAsyncCommand, IAsyncRollback
    {
       
    }
    
    public interface IAsyncRollbackCommand<T> : IAsyncCommand<T>,IAsyncRollback<T>
    {
        
    }
    
    public interface IAsyncRollbackCommand<TCommand,TRollback> : IAsyncCommand<TCommand>,IAsyncRollback<TRollback>
    {
        
    }
}