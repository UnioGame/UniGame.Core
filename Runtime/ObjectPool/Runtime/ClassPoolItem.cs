namespace UniGame.Runtime.ObjectPool
{
    using System;
    using System.Collections.Concurrent;

    public static class ClassPoolItem<T> 
        where T : class
    {
        [NonSerialized]
        private static ConcurrentQueue<T> _items = new ConcurrentQueue<T>();
        
        public static Type Type = typeof(T);

        public static int Count => _items.Count;

        public static bool IsEmpty => _items.IsEmpty;
        
        #region construcotr
        
        static ClassPoolItem()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlaymodeChanged;
#endif
        }
		
#if UNITY_EDITOR
        private static void OnPlaymodeChanged(UnityEditor.PlayModeStateChange change)
        {
            Release();
        }
#endif
        
        #endregion
        
        public static void Release()
        {
            _items = new ConcurrentQueue<T>();
        }
		
        public static void Enqueue(T item)
        {
            _items.Enqueue(item);
        }

        public static T Dequeue()
        {
            return !_items.TryDequeue(out var item) ? default : item;
        }
    }
}