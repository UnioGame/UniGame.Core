namespace UniGame.Core.Editor
{
    public interface IEditorProcess
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}