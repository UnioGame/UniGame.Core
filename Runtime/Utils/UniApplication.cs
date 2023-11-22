namespace UniModules.UniGame.Core.Runtime.Utils
{
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public static class UniApplication
    {
        private static bool _isPlaying = true;
        private static UniPlayModeState _playModeState = UniPlayModeState.EnteredPlayMode;

        public static UniPlayModeState PlayModeState
        {
            get
            {
#if !UNITY_EDITOR
                return UniPlayModeState.EnteredPlayMode;
#endif
                return _playModeState;
            }
            set
            {
                _playModeState = value;
            }
        }

        public static bool IsPlaying
        {
            get => _isPlaying;
            private set => _isPlaying = value;
        }


#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        static void Initialize()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= PlayModeChanged;
            EditorApplication.playModeStateChanged += PlayModeChanged;
            _isPlaying = EditorApplication.isPlaying;
#endif
        }
        
        
#if UNITY_EDITOR
        private static void PlayModeChanged(PlayModeStateChange state)
        {
            var playState = Convert(state);
            _playModeState = playState;
            
            switch (playState) {
                case UniPlayModeState.ExitingEditMode:
                case UniPlayModeState.EnteredEditMode:
                    IsPlaying = false;
                    break;
                case UniPlayModeState.ExitingPlayMode:
                case UniPlayModeState.EnteredPlayMode:
                    IsPlaying = true;
                    break;
            }
        }
#endif

#if UNITY_EDITOR
        private static UniPlayModeState Convert(PlayModeStateChange playModeState)
        {
            switch (playModeState)
            {
                case PlayModeStateChange.EnteredEditMode:
                    return UniPlayModeState.EnteredEditMode;
                case PlayModeStateChange.ExitingEditMode:
                    return UniPlayModeState.ExitingEditMode;
                case PlayModeStateChange.EnteredPlayMode:
                    return UniPlayModeState.EnteredPlayMode;
                case PlayModeStateChange.ExitingPlayMode:
                    return UniPlayModeState.ExitingPlayMode;
            }

            return UniPlayModeState.EnteredEditMode;
        }
#endif
    }
}
