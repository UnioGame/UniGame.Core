using System.Threading;

namespace UniModules.UniCore.Runtime.DataFlow
{
    using System;
    using Interfaces;
    using ObjectPool.Runtime.Interfaces;
    using UniGame.Core.Runtime.DataFlow;
    using UniGame.Core.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

    public class LifeTimeDefinition : 
        IUnique, 
        ILifeTime, 
        IPoolable
    {
        private LifeTime lifeTime;
        private int id;
        
        public LifeTimeDefinition()
        {
            lifeTime = DataFlow.LifeTime.Create();
            id = Unique.GetId();
        }
        
        public bool IsTerminated => lifeTime.isTerminated;

        public CancellationToken TokenSource => lifeTime.TokenSource;

        public ILifeTime LifeTime => lifeTime;

        public int Id => id;
        
        public void Terminate() => lifeTime.Release();

        public void Release() => lifeTime.Restart();
        
        
        #region ilifetime api

        public ILifeTime AddCleanUpAction(Action cleanAction) => lifeTime.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => lifeTime.AddDispose(item);

        public ILifeTime AddRef(object o) => lifeTime.AddRef(o);
        
        #endregion

    }
}
