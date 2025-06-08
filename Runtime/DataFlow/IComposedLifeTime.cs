namespace UniGame.DataFlow
{
    using System;
    using System.Collections.Generic;
    using global::UniGame.Core.Runtime;

    public interface IComposedLifeTime : ILifeTime, IDisposable
    {
        IComposedLifeTime Bind(ILifeTime lifeTime);
        IComposedLifeTime Bind(IEnumerable<ILifeTime> lifeTimes);
    }
}