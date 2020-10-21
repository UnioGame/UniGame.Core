namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;

    public interface ICommand
    {
        void Execute();
    }
    
    public interface ICommand<TResult>
    {
        TResult Execute();
    }

    public interface ICommand<TValue,TResult>
    {
        TResult Execute(TValue value);
    }
    
    public interface IDisposableCommand : ICommand, IDisposable{}
    
    public interface IRoutine<TResult> : ICommand<TResult>
    {
    }

    public interface IRoutine<TData,TResult> : ICommand<TData,TResult>
    {
    }
}