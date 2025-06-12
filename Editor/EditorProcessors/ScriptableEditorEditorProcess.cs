﻿namespace UniGame.Core.Editor
{
    using UnityEngine;

    public abstract class ScriptableEditorEditorProcess : ScriptableObject, IEditorProcess
    {
        public     abstract     bool IsRunning { get; }
        
        public abstract void Start();
        public abstract void Stop();
    }
}