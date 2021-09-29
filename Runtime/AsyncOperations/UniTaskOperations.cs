namespace UniModules.UniCore.Runtime.AsyncOperations {
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;


    public static class UniTaskOperations {
        
        public static UniTask CancelledTask =  new UniTask(new CanceledTaskResultSource(CancellationToken.None), 0);

        public static async UniTask AwaitWhileAsync(this object source, Func<bool> awaitFunc, CancellationToken cancellationToken, PlayerLoopTiming timing = PlayerLoopTiming.Update)
            => await UniTask.WaitWhile(awaitFunc,timing,cancellationToken);

        public static async UniTask<TResult> AwaitAsync<TResult>(Func<bool> awaitFunc, Func<TResult> resultFunc,CancellationToken cancellationToken) {
            if (awaitFunc == null)
                return resultFunc();
            
            await UniTask.WaitWhile(awaitFunc,PlayerLoopTiming.Update,cancellationToken);
            //return current state result
            return resultFunc();
        }
    }
}