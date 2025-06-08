using System.Threading;

namespace UniGame.Runtime.DataFlow
{
    using System;
    using global::UniGame.Core.Runtime.ObjectPool;
    using global::UniGame.DataFlow;
    using global::UniGame.Core.Runtime;

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

        public CancellationToken Token => lifeTime.Token;

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
