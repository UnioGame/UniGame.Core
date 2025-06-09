using System;
using UniGame.GameFlow.Runtime;

namespace UniGame.UniNodes.GameFlow.Runtime
{
    using Cysharp.Threading.Tasks;
    using UniGame.Runtime.DataFlow;
    using Core.Runtime;

    /// <summary>
    /// base game service class for binding Context source data to service logic
    /// </summary>
    [Serializable]
    public abstract class GameService : IGameService
    {
        private readonly LifeTimeDefinition _lifeTimeDefinition = new();

        public void Dispose() => _lifeTimeDefinition.Terminate();

        public ILifeTime LifeTime => _lifeTimeDefinition;

        public virtual UniTask InitializeAsync() { return UniTask.CompletedTask; }
    }
}