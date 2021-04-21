namespace UniModules.UniGame.Core.Runtime.Utils
{
    public static class UniApplication
    {
        public static bool IsPlaying = true;

        static UniApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeChanged;
#endif
        }
        
#if UNITY_EDITOR
        
        private void PlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            switch (state) {
                case UnityEditor.PlayModeStateChange.ExitingPlayMode:
                    Reset();
                    break;
            }
        }
        
#endif
    }
}
