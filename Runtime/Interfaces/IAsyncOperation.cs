namespace UniModules.UniGame.Core.Runtime.Interfaces
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IAsyncOperation : IPoolable, ICommandRoutine
    {
        bool IsDone { get; }
        string Error { get; }
    }
}
