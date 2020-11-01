namespace UniModules.UniCore.Runtime.AsyncOperations {
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public class CanceledTaskResultSource : IUniTaskSource
    {
        readonly CancellationToken cancellationToken;

        public CanceledTaskResultSource(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        public void GetResult(short token)
        {
            throw new OperationCanceledException(cancellationToken);
        }

        public UniTaskStatus GetStatus(short token)
        {
            return UniTaskStatus.Canceled;
        }

        public UniTaskStatus UnsafeGetStatus()
        {
            return UniTaskStatus.Canceled;
        }

        public void OnCompleted(Action<object> continuation, object state, short token)
        {
            continuation(state);
        }
    }
}