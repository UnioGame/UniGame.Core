using UniGame.Core.Runtime.ObjectPool;
using UniGame.Core.Runtime;

namespace UniGame.Common
{
    using System;
    using System.Threading;
    using Runtime.DataFlow;

    public class DisposableLifetime : IDisposableLifetime, IPoolable
    {
        private LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        private ILifeTime lifeTime;
        private bool isCompleted = true;
        
        public ILifeTime LifeTime => lifeTime;

        public bool IsComplete => isCompleted;

        /// <summary>
        /// restart disposable
        /// </summary>
        public IDisposableLifetime Initialize()
        {
            lifeTime = lifeTimeDefinition.LifeTime;
            lifeTimeDefinition.Release();
            isCompleted = false;
            lifeTime.AddCleanUpAction(Complete);
            
            return this;
        }

        public void Dispose() => Release();

        public void Release()
        {
            isCompleted = true;
            lifeTime    = null;
            lifeTimeDefinition.Terminate();
        }

        public void Complete() => Release();
        
        #region lifetime api

        public bool IsTerminated => lifeTime == null || lifeTime.IsTerminated;
        
        public CancellationToken Token => lifeTimeDefinition.Token;

        public ILifeTime AddCleanUpAction(Action cleanAction) => lifeTime.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => lifeTime.AddDispose(item);

        public ILifeTime AddRef(object o) => lifeTime.AddRef(o);

        #endregion
    }
}
