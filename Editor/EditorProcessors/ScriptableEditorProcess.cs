namespace UniModules.UniGame.Core.Editor.EditorProcessors
{
    using UnityEngine;

    public abstract class ScriptableEditorProcess : ScriptableObject, IProcess
    {
        public     abstract     bool IsRunning { get; }
        
        public abstract void Start();
        public abstract void Stop();
    }
}