namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    public interface IEditorProcess
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}