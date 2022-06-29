namespace UniGame.Utils.Abstract
{
    using System;
    using System.Threading;

    public interface ITaskExecutor<T> : IDisposable
    {
        CancellationTokenSource CancellationTokenSource { get; }

        IObservable<TaskExecutionResult<T>> TryExecute();
        
        void RaiseSuccessfulLoad(T result);
        void RaiseFailedLoad();
    }

    public class TaskExecutionResult<T>
    {
        public T Result { get; }
        public bool IsSuccessful { get; }

        public TaskExecutionResult(T result, bool isSuccessful)
        {
            Result = result;
            IsSuccessful = isSuccessful;
        }
    }
}