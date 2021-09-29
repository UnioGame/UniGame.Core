namespace UniModules.UniGame.Core.Runtime.Rx
{
    using System;
    using System.Threading;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public sealed class UniObserverNode<T> : IObserver<T>, IDisposable, IPoolable
    {
        private bool                      _isDisposed = false;
        private IObserver<T>              _observer;
        private IUniObserverLinkedList<T> _list;

        public UniObserverNode<T> Previous { get; internal set; }
        public UniObserverNode<T> Next     { get; internal set; }

        public UniObserverNode<T> Initialize(IUniObserverLinkedList<T> list, IObserver<T> observer)
        {
            _isDisposed = false;
            _list       = list;
            _observer   = observer;
            return this;
        }

        public void OnNext(T value) => _observer.OnNext(value);

        public void OnError(Exception error) =>  _observer.OnError(error);

        public void OnCompleted() => _observer.OnCompleted();

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;
            var sourceList = Interlocked.Exchange(ref _list, null);
            if (sourceList == null)
                return;
            
            sourceList.UnsubscribeNode(this);
            
            this.Despawn();
        }

        public void Release()
        {
            _isDisposed = false;
            _observer   = null;
            _list       = null;
        }
    }
}