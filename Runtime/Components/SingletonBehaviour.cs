﻿namespace UniModules.UniGame.Core.Runtime.Components
{
    using global::UniGame.Core.Runtime;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    public class SingletonBehaviour<T> : MonoBehaviour, INamedItem where T : SingletonBehaviour<T>
    {
#region statics data
        private static readonly object _lock = new object();
        private static T _instance;

        public static bool Exists => _instance != null;
        
        public static T Instance {
            get {
                lock (_lock) {
                    if (!_instance || _instance == null) {
                        _instance = FindObjectOfType<T>() ?? CreateInstance();
                    }
                }
                return _instance;
            }
        }

#if UNITY_EDITOR
        
        [InitializeOnLoadMethod]
        public static void InitializeStatic()
        {
            EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        private static void OnPlaymodeStateChanged(PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                    _instance = null;
                    break;
            }
        }
        
#endif
        
#endregion
        
        public virtual string ItemName => typeof(T).Name;

        public static void DestroySingleton()
        {
            if (!Exists) return;
            DestroyImmediate(Instance.gameObject);
            _instance = null;
        }

        protected virtual void OnInstanceCreated()
        {
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this) {
                DestroyImmediate(gameObject);
                return;
            }
            
            _instance = this as T;
        }

        private static T CreateInstance()
        {
            var singletonObject = new GameObject();

            if (Application.isPlaying) {
                DontDestroyOnLoad(singletonObject);
                singletonObject.hideFlags = HideFlags.DontSave;
            }
            else {
                singletonObject.hideFlags = HideFlags.HideAndDontSave;
            }

            var instance = singletonObject.AddComponent<T>();
            singletonObject.name = instance.ItemName;
            
            instance.OnInstanceCreated();

            return instance;
        }

    }
}