using System.Collections.Generic;

namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using System.Linq;
    using System.Diagnostics;
    using DataFlow.Interfaces;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    public static class LifetimeObjectData
    {
        private static List<ILifeTime> _lifetimes = new List<ILifeTime>();
        private static List<LifetimeScriptableObject> _lifetimeScriptableObjects = new List<LifetimeScriptableObject>();

        public static List<ILifeTime> LifeTimes => _lifetimes;

        public static bool IsReportingEnabled { get; set; } = false;
        
        [Conditional("UNITY_EDITOR")]
        public static void Add(ILifeTime lifetime)
        {
            if (Find(lifetime) != null)
                return;
            
            _lifetimes.Add(lifetime);
            if (lifetime is LifetimeScriptableObject lifetimeScriptableObject)
                _lifetimeScriptableObjects.Add(lifetimeScriptableObject);
        }

        
        [Conditional("UNITY_EDITOR")]
        public static void Remove(ILifeTime lifetime)
        {
            var reference = Find(lifetime);
            if (reference == null) return;
            
            _lifetimes.Remove(reference);
            if (reference is LifetimeScriptableObject lifetimeScriptableObject)
                _lifetimeScriptableObjects.Remove(lifetimeScriptableObject);
        }

        public static ILifeTime Find(ILifeTime lifeTime)
        {
            return _lifetimes.FirstOrDefault(x => x == lifeTime);
        }

        public static void Reset()
        {
            _lifetimes.RemoveAll(x => x == null);
            _lifetimeScriptableObjects.RemoveAll(x => x == null);

            foreach (var lifetime in _lifetimeScriptableObjects)
                lifetime.Reset();
        }
        
#if UNITY_EDITOR
        

        [InitializeOnLoadMethod]
        public static void Inintialize()
        {
            EditorApplication.playModeStateChanged -= PlayModeChanged;
            EditorApplication.playModeStateChanged += PlayModeChanged;
        }

        private static void PlayModeChanged(PlayModeStateChange state)
        {
            switch (state) {
                case PlayModeStateChange.ExitingPlayMode:
                    Reset();
                    break;
            }
        }
        
#endif
    }
}
