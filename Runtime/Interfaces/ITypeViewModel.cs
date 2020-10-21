namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using System;
    using DataFlow.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface ITypeViewModel : IPoolable
    {
        /// <summary>
        /// is view model already initialized
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// view lifetime
        /// </summary>
        ILifeTime LifeTime { get; }

        /// <summary>
        /// model type
        /// </summary>
        Type Type { get; }
    }
}