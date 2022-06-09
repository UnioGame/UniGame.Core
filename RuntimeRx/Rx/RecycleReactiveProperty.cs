namespace UniModules.UniGame.Core.Runtime.Rx
{
    using System;
    using System.Collections.Generic;
    using global::UniGame.Core.Runtime.Utils;
    using Interfaces.Rx;
    using UniCore.Runtime.Attributes;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public class RecycleReactiveProperty<T> :
        IRecycleReactiveProperty<T>,
        IUniObserverLinkedList<T>,
        IDespawnable
    {
        private IEqualityComparer<T>        _equalityComparer;

        #region inspector

        [SerializeField] 
        protected T value = default;

        [Tooltip("Mark this field to true, if you want notify immediately after subscription")]
        [ReadOnlyValue]
        [SerializeField]
        protected bool hasValue = false;

        #endregion

        private bool _isDisposed = false;
        
        [NonSerialized] private UniObserverNode<T> _root;

        [NonSerialized] private UniObserverNode<T> _last;

        #region constructor

        public RecycleReactiveProperty()
        {
        }

        public RecycleReactiveProperty(T value)
        {
            this.value = value;
            hasValue   = true;
        }

        #endregion

        IEqualityComparer<T> EqualityComparer => _equalityComparer ??= CreateComparer();

        public T Value
        {
            get => value;
            set => SetValue(value);
        }

        public bool HasValue => hasValue;

        public Type Type => typeof(T);

        #region public methods

        public void MakeDespawn()
        {
            Release();
            this.Despawn();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_isDisposed)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            //if value already exists - notify
            if (hasValue) observer.OnNext(value);

            var next = new UniObserverNode<T>(this, observer);

            if (_root == null)
            {
                _root = _last = next;
            }
            else
            {
                _last.Next    = next;
                next.Previous = _last;
                _last         = next;
            }

            return next;
        }

        public void UnsubscribeNode(UniObserverNode<T> node)
        {
            if (node == _root)
                _root = node.Next;

            if (node == _last)
                _last = node.Previous;

            if (node.Previous != null)
                node.Previous.Next = node.Next;

            if (node.Next != null)
                node.Next.Previous = node.Previous;
        }

        public void Dispose()
        {
            _isDisposed = true;
            CleanUp();
        }

        public void SetValue(T propertyValue)
        {
            if (hasValue && EqualityComparer.Equals(value, propertyValue))
                return;

            SetValueForce(propertyValue);
        }

        public void SetValueForce(T propertyValue)
        {
            hasValue = true;
            value    = propertyValue;
            RaiseOnNext(ref propertyValue);
        }

        public void SetValueSilence(T propertyValue)
        {
            hasValue = true;
            value    = propertyValue;
        }

        public void RemoveValueSilence()
        {
            hasValue = false;
            value    = default;
        }

        public void Release()
        {
            _isDisposed = false;
            CleanUp();
        }

        public object GetValue() => value;

        public void SetObjectValue(object nextValue)
        {
            if (nextValue is T targetValue)
                SetValue(targetValue);
        }

        #endregion

        private void RaiseOnNext(ref T nextValue)
        {
            var node = _root;
            while (node != null)
            {
                var nodeBuffer = node.Next;
                node.OnNext(nextValue);
                node = nodeBuffer;
            }
        }

        protected virtual IEqualityComparer<T> CreateComparer() => UnityEqualityComparer.GetDefault<T>();

        private void CleanUp()
        {
            value    = default;
            hasValue = false;

            var node      = _root;
            _root = _last = null;

            while (node != null)
            {
                node.OnCompleted();
                node = node.Next;
            }
        }
    }
}