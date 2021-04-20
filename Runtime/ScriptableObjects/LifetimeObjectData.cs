using System.Linq;

namespace UniModules.UniGame.Core.Runtime.ScriptableObjects
{
    using System.Diagnostics;
    using DataFlow.Interfaces;
    using UniRx;

    public static class LifetimeObjectData
    {
        private static ReactiveCollection<ILifeTime> _lifetimes = new ReactiveCollection<ILifeTime>();

        public static IReadOnlyReactiveCollection<ILifeTime> LifeTimes => _lifetimes;

        public static bool IsReportingEnabled { get; set; } = false;
        
        [Conditional("UNITY_EDITOR")]
        public static void Add(ILifeTime lifetime)
        {
#if UNITY_EDITOR
            
            if (Find(lifetime) != null)
                return;
            
            _lifetimes.Add(lifetime);
            
#endif
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void Remove(ILifeTime lifetime)
        {
            var reference = Find(lifetime);
            if (reference != null)
                _lifetimes.Remove(reference);
        }

        public static ILifeTime Find(ILifeTime lifeTime)
        {
            return _lifetimes.FirstOrDefault(x => x == lifeTime);
        }
    }
}
