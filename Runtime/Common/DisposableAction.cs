namespace UniModules.UniCore.Runtime.Common
{
    using System;
    using ObjectPool;
    using ObjectPool.Runtime.Extensions;
    using ObjectPool.Runtime.Interfaces;
    using UniGame.Core.Runtime.Interfaces;

    public class DisposableAction : IDisposableItem , IPoolable
    {
        private Action _onDisposed;

        public bool IsComplete { get; private set; } = true;
    
        public void Initialize(Action action)
        {
            IsComplete = false;
            _onDisposed = action;
        }

        public void Dispose()
        {
            if (IsComplete) return;
            IsComplete = true;
            
            _onDisposed?.Invoke();
            
            Complete();
            
            this.DespawnWithRelease();
        }

        public void Complete()
        {
            IsComplete = true;
            _onDisposed = null;
        }

        public void Release() => Complete();
    }
}
