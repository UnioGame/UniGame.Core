namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;
    using Cysharp.Threading.Tasks;
    using DataFlow.Interfaces;
    using UniRx;

    public interface IAsyncController : IDisposable
    {
        
        IReadOnlyReactiveProperty<bool> IsInitialized { get; }

        ILifeTime LifeTime { get; }

        UniTask Initialize();
        
    }
}