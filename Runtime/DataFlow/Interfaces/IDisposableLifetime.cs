using System;

namespace UniModules.UniGame.Core.Runtime.Common
{
    using global::UniGame.Core.Runtime;

    public interface IDisposableLifetime : 
        IDisposableItem, 
        ILifeTimeContext, 
        ILifeTime
    {
    }

    public interface IDisposableLifetimeContext:
        ILifeTimeContext,
        IDisposable
    {
        
    }
}