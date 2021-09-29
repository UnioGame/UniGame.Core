namespace UniModules.UniGame.Core.Runtime.Rx
{
    using System;
    using System.Collections.Generic;
    using DataStructure;
    using global::UniGame.Core.Runtime.Utils;
    using Interfaces.Rx;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public class RecycleReactiveProperty<T> : 
        IRecycleReactiveProperty<T>  , 
        IUniObserverLinkedList<T>,
        IDespawnable
    {
        private IEqualityComparer<T> _equalityComparer;
        private LifeTimeDefinition _lifeTimeDefinition = new LifeTimeDefinition();
        private UniLinkedList<IObserver<T>> _observers;

        #region inspector
                
        [SerializeField]
        protected T value = default;
        
        [Tooltip("Mark this field to true, if you want notify immediately after subscription")]
        [SerializeField]
        protected bool hasValue = false;

        #endregion

        [NonSerialized]
        private UniObserverNode<T> _root;

        [NonSerialized]
        private UniObserverNode<T> _last;

        #region constructor

        public RecycleReactiveProperty(){}

        public RecycleReactiveProperty(T value)
        {
            this.value = value;
            hasValue = true;
        }
        
        #endregion

        public ILifeTime LifeTime => _lifeTimeDefinition ??= new LifeTimeDefinition();
        
        IEqualityComparer<T> EqualityComparer => _equalityComparer ??= CreateComparer();
        
        public T Value {
            get => value;
            set => SetValue(value);
        }

        public bool HasValue => hasValue;

        public Type Type => typeof(T);

        #region public methods
        
        public void MakeDespawn() {
            Release();
            this.Despawn();
        }
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (LifeTime.IsTerminated) {
                observer.OnCompleted();
                return Disposable.Empty;
            }
            
            //if value already exists - notify
            if(hasValue) observer.OnNext(Value);
            
            var next = ClassPool.Spawn<UniObserverNode<T>>()
                .Initialize(this,observer);

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
        
        public void Dispose() => _lifeTimeDefinition.Terminate();

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
            CleanUp();
            _lifeTimeDefinition.Release();
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
                node.OnNext(nextValue);
                node = node.Next;
            }
        }
        
        protected virtual IEqualityComparer<T> CreateComparer() => UnityEqualityComparer.GetDefault<T>();

        private void CleanUp()
        {
            value = default;
            hasValue = false;
            
            var node          = _root;
            _root       = _last = null;

            while (node != null)
            {
                node.OnCompleted();
                node = node.Next;
            }
        }

    }
}
