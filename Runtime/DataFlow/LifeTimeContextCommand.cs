namespace UniGame.Core.Runtime.DataFlow
{
    using System;
    using UniModules.UniCore.Runtime.DataFlow;
    using UniModules.UniCore.Runtime.DataFlow.Interfaces;
    using UniModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniModules.UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public class LifeTimeContextCommand : IDisposableCommand,IPoolable
    {
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private Action<ILifeTime> _action;

        public void Initialize(Action<ILifeTime> contextAction)
        {
            _lifeTime.Release();
            _action = contextAction;
        }

        public void Execute()
        {
            _lifeTime.Release();
            _action?.Invoke(_lifeTime);
        }

        public void Dispose() => this.DespawnWithRelease();

        public void Release()
        {
            _lifeTime.Release();
            _action = null;
        }
    }
}
