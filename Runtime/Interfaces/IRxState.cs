namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;
    using DataFlow.Interfaces;
    using UniRx;

    public interface IRxCommand<TValue, TResult>
    {
        IObservable<TResult> Execute(TValue value);
    }
    
    public interface IRxState<TValue> : IRxState<TValue,Unit>
    {
        
    }
    
    public interface IRxState<TValue,TResult>:
        IRxCommand<TValue,TResult>,
        IRxEndPoint,
        ILifeTimeContext,
        IActiveStatus
    {
        
    }

    public interface IRxEndPoint
    {
        void ExitState();
    }
    
    public interface IRxRolldback<TData>
    {
        IObservable<bool> Rollback(TData data);
    }
    
    public interface IRxRolldback
    {
        IObservable<bool> Rolldback();
    }
    
    public interface IRxCompletion<TData,TResult>
    {
        IObservable<Unit> CompleteAsync(TData data, TResult value,  ILifeTime lifeTime);
    }

    public interface IRxStateExecution<TData,TResult>
    {
        IObservable<TResult> ExecuteState(TData data, ILifeTime lifeTime);
    }
}