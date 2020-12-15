namespace UniModules.UniGame.Context.SerializableContext.Runtime.States
{
    using System;
    using Core.Runtime.AsyncOperations;
    using Core.Runtime.DataFlow.Interfaces;
    using Core.Runtime.Interfaces;
    using UniRx;

    [Serializable]
    public class RxStateProxy<TData> : RxStateProxy<TData, Unit>
    {

        public RxStateProxy(
            IRxStateExecution<TData, Unit> command = null,
            IRxCompletion<TData, Unit> onComplete = null,
            IEndPoint endPoint = null,
            IRxRolldback<TData> onRollback = null) : base(command, onComplete, endPoint, onRollback)
        {
            
        }
    }
    
    [Serializable]
    public class RxStateProxy<TData, TResult> : 
        RxState<TData,TResult>,
        IRxRolldback<TData>
    {
        private readonly IRxStateExecution<TData,TResult> _command;
        private readonly IRxCompletion<TData, TResult>    _onComplete;
        private readonly IEndPoint                        _endPoint;
        private readonly IRxRolldback<TData>              _onRollback;

        public RxStateProxy(
            IRxStateExecution<TData,TResult>  command = null,
            IRxCompletion<TData, TResult> onComplete = null,
            IEndPoint endPoint = null,
            IRxRolldback<TData>   onRollback = null)
        {
            _command    = command;
            _onComplete = onComplete;
            _endPoint   = endPoint;
            _onRollback = onRollback;
        }

        public IObservable<bool> Rollback(TData source)
        {
            return _onRollback != null ? 
                _onRollback.Rollback(source) : 
                Observable.Return(true);
        }
        
        
        protected sealed override IObservable<Unit> OnComplete(TData context,TResult value,  ILifeTime lifeTime)
        {
            return _onComplete == null ? 
                Observable.Return(Unit.Default) : 
                _onComplete.CompleteAsync(context,value, lifeTime);
        }
        
        protected sealed override IObservable<TResult> OnExecute(TData context, ILifeTime lifeTime)
        {
            return _command == null ? 
                Observable.Return(default(TResult)) : 
                _command.ExecuteState(context,lifeTime);
        }

        protected sealed override void OnExit() {
            _endPoint?.Exit();
        }
    }
}