using UniGame.Core.Runtime.ObjectPool;
using UniGame.Core.Runtime;

namespace UniGame.Core.Runtime
{
    using System;

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