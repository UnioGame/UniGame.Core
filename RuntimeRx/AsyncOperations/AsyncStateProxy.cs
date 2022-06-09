namespace UniModules.UniGame.Context.SerializableContext.Runtime.States {
    using System;
    using Core.Runtime.AsyncOperations;
    using Core.Runtime.DataFlow.Interfaces;
    using Cysharp.Threading.Tasks;


    [Serializable]
    public class AsyncStateProxy<TData,TValue> : 
        AsyncState<TData,TValue>
    {
        private readonly Func<TData, ILifeTime, UniTask<TValue>> onExecute;
        private readonly Func<TValue, TData, ILifeTime, UniTask> onComplete;
        private readonly Func<TData,UniTask>                     onExit;

        public AsyncStateProxy(
            Func<TData, ILifeTime, UniTask<TValue>> onExecute,
            Func<TValue, TData, ILifeTime, UniTask> onComplete,
            Func<TData,UniTask> onExit) {
            this.onExecute  = onExecute;
            this.onComplete = onComplete;
            this.onExit     = onExit;
        }

        protected override async UniTask OnComplete(TValue value, TData context, ILifeTime lifeTime) {
            if (onComplete == null)
                return;
            await onComplete(value,context,lifeTime);
        }
        
        protected override async UniTask<TValue> OnExecute(TData context, ILifeTime lifeTime) {
            if (onExecute == null)
                return default;
            return await onExecute(context,lifeTime);
        }

        protected override async UniTask OnExit(TData data) {
            if (onExit == null)
                return;
            await onExit(data);
        }
    }
    
}
