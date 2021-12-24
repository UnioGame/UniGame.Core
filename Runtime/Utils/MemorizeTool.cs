namespace UniModules.UniCore.Runtime.Utils
{
    using System;
    using UniRx;

    public static class MemorizeTool
    {
        private static ReactiveProperty<Unit> cleanupStream = new ReactiveProperty<Unit>();
        
        public static MemorizeItem<TKey,TData> Memorize<TKey,TData>(Func<TKey, TData> factory, Action<TData> disposableAction = null)
        {
            var cache = new MemorizeItem<TKey,TData>(factory,disposableAction);
#if UNITY_EDITOR
            //clean up cache if Assembly Reload
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += cache.Dispose;
            UnityEditor.EditorApplication.playModeStateChanged    += x => cache.Dispose();
            cleanupStream.Subscribe(x => cache.Dispose());
#endif
            return cache;
        }

        public static Func<TKey,TData> Create<TKey,TData>(Func<TKey,TData> factory, Action<TData> disposableAction = null) {

            var cache = Memorize(factory,disposableAction);

            return cache.GetValue;

        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("UniGame/Tools/Clear Memorize Cache")]
#endif
        public static void Clear()
        {
            cleanupStream.SetValueAndForceNotify(Unit.Default);
        }

    }
}
