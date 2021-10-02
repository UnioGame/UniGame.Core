using System;
using Cysharp.Threading.Tasks;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniCore.Runtime.Extension;
using UniModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces;
using UniModules.UniCore.Runtime.Rx.Extensions;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniRx;

namespace UniModules.UniGame.CoreModules.UniGame.Core.Runtime.Async
{
    public class AwaitFirstAsyncOperation<TData> : IPoolable
    {
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private bool _valueInitialized = false;
        private TData _value;
    
        public async UniTask<TData> AwaitFirstAsync(
            IObservable<TData> observable, 
            Func<TData,bool> predicate = null)
        {
            if (observable == null) 
                return default;

            observable.Subscribe(x => OnNext(x,predicate)).AddTo(_lifeTime);
            await this.WaitUntil(() => _lifeTime.IsTerminated || _valueInitialized);
            return _value;
        }

        public void Release()
        {
            _lifeTime.Release();
            _valueInitialized = false;
            _value = default;
        }
    
        private void OnNext(TData value,Func<TData,bool> predicate = null)
        {
            if (predicate != null && !predicate.Invoke(value))
                return;
        
            _valueInitialized = true;
            _value = value;
        }

    }
}
