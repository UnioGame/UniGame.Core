namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    public interface IProcess
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}