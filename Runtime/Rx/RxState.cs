namespace UniModules.UniGame.Core.Runtime.AsyncOperations
{
    using System;
    using DataFlow.Interfaces;
    using global::UniCore.Runtime.ProfilerTools;
    using Interfaces;
    using Rx;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;

    public class RxState<TData> : RxState<TData, Unit>,
        IRxState<TData>
    {
        
    }
    
    public class RxState<TData, TResult> : IRxState<TData, TResult>
    {
        private LifeTimeDefinition   _lifeTime;
        private bool                 _isActive;
        private bool                 _isInitialized;
        private Exception            _exception;
        private IObservable<TResult> _executionObservable;
        
        private RecycleReactiveProperty<TResult> _value            = new RecycleReactiveProperty<TResult>(); 

        #region public properties

        public IReadOnlyReactiveProperty<TResult> Value => _value = (_value ?? new RecycleReactiveProperty<TResult>());

        public ILifeTime LifeTime => _lifeTime = (_lifeTime ?? new LifeTimeDefinition());

        public bool IsActive => _isActive;

        #endregion

        public IObservable<TResult> Execute(TData data)
        {
            //state already active
            if (_isActive)
                return _value;
            
            _isActive = true;

            if (!_isInitialized)
                Initialize();

            //cleanup value on reset
            LifeTime.AddCleanUpAction(() => _value.Release());
            //setup default value
            _value.Value = GetInitialExecutionValue();

            _executionObservable = OnExecute(data, _lifeTime);
            
            _executionObservable
                .Do(x => _value.SetValue(x))
                .DoOnCompleted(
                    () => OnComplete(data,_value.Value,_lifeTime)
                        .Subscribe()
                        .AddTo(_lifeTime))
                .Subscribe()
                .AddTo(_lifeTime);

            _executionObservable
                .DoOnError(
                    x => OnError(x,data)
                        .Subscribe()
                        .AddTo(_lifeTime))
                .Subscribe()
                .AddTo(_lifeTime);

            return _executionObservable;
        }

        public void ExitState()
        {
            if (!_isActive)
                return;
            Finish();
        }

        private void Finish()
        {
            OnExit();

            _isActive = false;
            _lifeTime?.Release();
        }

        protected virtual TResult GetInitialExecutionValue()
        {
            return default;
        }

        protected virtual IObservable<TResult> OnExecute(TData data, ILifeTime executionLifeTime) => Observable.Return(default(TResult));

        protected virtual IObservable<Unit> OnComplete(TData data,TResult value, ILifeTime lifeTime) => Observable.Return(Unit.Default);

        protected virtual void OnExit() { return; }

        private IObservable<bool> OnError(Exception exception,TData data)
        {
            GameLog.LogError($"RxState Error: {exception}");
            
            switch (this)
            {
                case IRxRolldback rolldback:
                    return rolldback.Rolldback();
                case IRxRolldback<TData> dataRollback:
                    return dataRollback.Rollback(data);
            }
            
            return Observable.Return(true);
        }
        
        private void Initialize()
        {
            _isInitialized = true;
            _lifeTime      = (_lifeTime ?? new LifeTimeDefinition());
            _value         = (_value ?? new RecycleReactiveProperty<TResult>());
        }
    }
}