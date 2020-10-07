namespace UniModules.UniGame.Core.Runtime.DataFlow
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UniModules.UniCore.Runtime.DataFlow.Interfaces;

    public interface IComposedLifeTime : ILifeTime, IDisposable
    {
        IComposedLifeTime Bind(ILifeTime lifeTime);
        IComposedLifeTime Bind(IEnumerable<ILifeTime> lifeTimes);
    }
}