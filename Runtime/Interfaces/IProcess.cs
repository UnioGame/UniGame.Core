namespace UniGame.Core.Runtime
{
    public interface IProcess
    {
        bool IsActive { get; }
        void Execute();
        void Stop();
    }
}