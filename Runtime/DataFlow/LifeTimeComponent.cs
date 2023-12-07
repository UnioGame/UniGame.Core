using System.Threading;
using UnityEngine;

namespace UniModules.UniGame.Core.Runtime.DataFlow
{
    using System;
    using global::UniGame.Core.Runtime;
    using UniModules.UniCore.Runtime.DataFlow;
    using UnityEngine.Serialization;

    public class LifeTimeComponent :
        MonoBehaviour, 
        ILifeTime
    {
        private readonly LifeTimeDefinition _lifeTime = new();
        private readonly LifeTimeDefinition _disableLifeTime = new();

        public ILifeTime AddCleanUpAction(Action cleanAction) => _lifeTime.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => _lifeTime.AddDispose(item);

        public ILifeTime AddRef(object o) => _lifeTime.AddRef(o);

        public ILifeTime DisableLifeTime => _disableLifeTime;
        
        public bool IsTerminated => _lifeTime.IsTerminated;
        
        public CancellationToken Token => _lifeTime.Token;

        private void OnEnable()
        {
            _disableLifeTime.Release();
        }

        private void OnDisable()
        {
            _disableLifeTime.Terminate();
        }

        private void OnDestroy()
        {
            _lifeTime.Terminate();
            _disableLifeTime.Terminate();
        }
    }
}
