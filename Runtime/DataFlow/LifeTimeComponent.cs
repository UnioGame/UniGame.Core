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
        [FormerlySerializedAs("releaseOnDisable")]
        public bool isReleaseOnDisable;
        
        private readonly LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        public ILifeTime AddCleanUpAction(Action cleanAction) => _lifeTime.AddCleanUpAction(cleanAction);

        public ILifeTime AddDispose(IDisposable item) => _lifeTime.AddDispose(item);

        public ILifeTime AddRef(object o) => _lifeTime.AddRef(o);

        public bool IsTerminated => _lifeTime.IsTerminated;
        
        public CancellationToken Token => _lifeTime.Token;

        private void OnEnable()
        {
            if (isReleaseOnDisable)
                _lifeTime.Release();
        }

        private void OnDisable()
        {
            if (isReleaseOnDisable)
                _lifeTime.Terminate();
        }

        private void OnDestroy()
        {
            _lifeTime.Terminate();
        }
    }
}
