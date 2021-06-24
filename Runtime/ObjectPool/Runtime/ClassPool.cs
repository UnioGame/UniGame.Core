namespace UniModules.UniCore.Runtime.ObjectPool.Runtime
{
	using System;
	using Interfaces;
	using ProfilerTools;
	using UnityEngine;
	using Object = UnityEngine.Object;

	public static class ClassPool {

		private static IPoolContainer _container;
		private static IPoolContainer Container => _container = _container ?? CreateContainer();
		
#region constructor		        
		static ClassPool()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += OnPlaymodeChanged;
#endif
		}
		
#if UNITY_EDITOR
		private static void OnPlaymodeChanged(UnityEditor.PlayModeStateChange change)
		{
			_container = null;
		}
#endif
		
#endregion

		public static TResult Spawn<TResult>(this object _, Action<TResult> onSpawn = null)
			where TResult : class, new()
		{
			return Spawn<TResult>(onSpawn);
		}

		public static TResult Spawn<TResult>(Action<TResult> onSpawn = null)
            where TResult : class, new()
        {
	        var item = SpawnExists(onSpawn);

	        if (item == null)
	        {
		        return new TResult();
	        }
            
	        return item;
        }

        public static TResult SpawnOrCreate<TResult>(Func<TResult> factory,Action<TResult> onSpawn = null)
            where TResult : class
        {
            var item = SpawnExists(onSpawn);
            	        
            if (item == null && factory!=null)
                item = factory();
	        
            return item;
        }

		public static T SpawnExists<T>()
			where T : class 
		{
			var instance = SpawnExists<T>(null);
            return instance;
        }

		public static T SpawnExists<T>(Action<T> onSpawn)
			where T : class 
		{
			if (!Container.Contains<T>())
		        return null;
			// Get the matched index, or the last index
		    var item = Container.Pop<T>();

		    // Run action?
			onSpawn?.Invoke(item);
			
			return item;
		}

		public static void Despawn<T>(T instance, Action<T> onDespawn)
			where T : class 
		{
			// Run action on it?
			onDespawn?.Invoke(instance);
			
			Despawn(instance);
		}

		public static void Despawn<T>(T instance)
			where T:class
		{
			if (instance == null)
				return;

			if(instance is IPoolable poolable) 
				poolable.Release();

			// Add to _cache
			Container.Push(instance);
		}

		public static void Despawn<T>(ref T instance, T defaultValue = null) where T : class
		{
			if (instance == null)
				return;
			Despawn(instance);
			instance = defaultValue;
		}

		private static readonly DummyPoolContainer dummyPoolContainer = new DummyPoolContainer();

		private static IPoolContainer CreateContainer()
		{
			if (!Application.isPlaying)
				return dummyPoolContainer;

			var container = new ClassPoolContainer();
			return container;
		}
	}
}