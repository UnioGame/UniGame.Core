namespace UniModules.UniGame.Core.Runtime.Rx
{
    using System;
    using System.Threading;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public sealed class UniObserverNode<T> : IObserver<T>, IDisposable, IPoolable
    {
        private IObserver<T>              _observer;
        private IUniObserverLinkedList<T> _list;

        public UniObserverNode<T> Previous;
        public UniObserverNode<T> Next;

        public UniObserverNode(IUniObserverLinkedList<T> list, IObserver<T> observer)
        {
            _list       = list;
            _observer   = observer;
        }

        public void OnNext(T value) => _observer.OnNext(value);

        public void OnError(Exception error) =>  _observer.OnError(error);

        public void OnCompleted() => _observer.OnCompleted();

        public void Dispose()
        {
            var sourceList = Interlocked.Exchange(ref _list, null);
            if (sourceList == null)
                return;
            
            sourceList.UnsubscribeNode(this);
            sourceList = null;
        }

        public void Release()
        {
            Dispose();
        }
    }
}