using System;

namespace UniGame.UniNodes.GameFlow.Runtime
{
    using Cysharp.Threading.Tasks;
    using UniModules.UniCore.Runtime.DataFlow;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UniModules.UniGameFlow.GameFlow.Runtime.Interfaces;
    using UniRx;

    /// <summary>
    /// base game service class for binding Context source data to service logic
    /// </summary>
    [Serializable]
    public abstract class GameService : IGameService, ICompletionSource
    {
        private readonly LifeTimeDefinition _lifeTimeDefinition = new LifeTimeDefinition();

        /// <summary>
        /// ready by default
        /// </summary>
        private readonly BoolReactiveProperty _isReady = new BoolReactiveProperty(false);

        /// <summary>
        /// complete service awaiter to mark it as ready
        /// </summary>
        public void Complete() => _isReady.Value = true;

        /// <summary>
        /// terminate service lifeTime to release resources
        /// </summary>
        public void Dispose() => _lifeTimeDefinition.Terminate();

        public bool IsComplete => _isReady.Value;

        public ILifeTime LifeTime => _lifeTimeDefinition;

        public IReadOnlyReactiveProperty<bool> IsReady => _isReady;

        public virtual UniTask InitializeAsync() { return UniTask.CompletedTask; }
    }
}