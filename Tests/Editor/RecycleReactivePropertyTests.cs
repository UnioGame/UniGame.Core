namespace UniModules.UniGame.CoreModules.UniGame.Core.Tests.Editor
{
    using System;
    using NUnit.Framework;
    using UniModules.UniGame.Core.Runtime.Rx;
    using UniRx;
    
    public class RecycleReactivePropertyTests
    {
        [Test]
        public void OnNextObserverDisposed()
        {
            var observable = new RecycleReactiveProperty<bool>();
            
            IDisposable observerToDispose = null;
            observable.Subscribe(_ => observerToDispose?.Dispose());
            observerToDispose = observable.Subscribe();

            observable.Value = true;
        }
        
        [Test]
        public void OnNextObserverDisposedNoRecycle()
        {
            var observable = new ReactiveProperty<bool>();
            
            IDisposable observerToDispose = null;
            observable.Subscribe(_ => observerToDispose?.Dispose());
            observerToDispose = observable.Subscribe();

            observable.Value = true;
        }
        
        [Test]
        public void OnNextObserverDisposedReverse()
        {
            var observable = new RecycleReactiveProperty<bool>();
            
            var observerToDispose = observable.Subscribe();
            observable.Subscribe(_ => observerToDispose?.Dispose());

            observable.Value = true;
        }
        
        [Test]
        public void OnNextObserverDisposedReverseNoRecycle()
        {
            var observable = new ReactiveProperty<bool>();
            
            var observerToDispose = observable.Subscribe();
            observable.Subscribe(_ => observerToDispose?.Dispose());

            observable.Value = true;
        }
    }
}