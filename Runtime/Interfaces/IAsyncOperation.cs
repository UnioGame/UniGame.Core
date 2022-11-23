namespace UniGame.Core.Runtime
{
    using UniGame.Core.Runtime.ObjectPool;
    
    public interface IAsyncOperation : IPoolable, ICommandRoutine
    {
        bool IsDone { get; }
        string Error { get; }
    }
}
