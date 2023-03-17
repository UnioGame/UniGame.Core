using UnityEngine;

namespace UniGame.Runtime.ObjectPool
{
	using System.Runtime.CompilerServices;
	using UniCore.Runtime.ProfilerTools;
	using UniGame.Core.Runtime.ObjectPool;
	using Unity.IL2CPP.CompilerServices;
	using UnityEngine.Pool;
	using System;

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public static class ClassPool
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TResult Spawn<TResult>(this object _, Action<TResult> onSpawn = null)
			where TResult : class, new()
		{
			return Spawn(onSpawn);
		}

		public static TResult Spawn<TResult>(Action<TResult> onSpawn = null)
            where TResult : class, new()
		{
			var item = Spawn<TResult>();
			onSpawn?.Invoke(item);
	        return item;
        }
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TResult Spawn<TResult>()
			where TResult : class, new()
		{
			var item = GenericPool<TResult>.Get();
			return item;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Despawn<T>(T instance, Action onDespawn)
			where T : class, new()
		{
			// Run action on it?
			onDespawn?.Invoke();
			Despawn(instance);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Despawn<T>(T instance, Action<T> onDespawn)
			where T : class, new()
		{
			// Run action on it?
			onDespawn?.Invoke(instance);
			Despawn(instance);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DespawnWithRelease<T>(T instance)
			where T:class, new()
		{
			Despawn(instance);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Despawn<T>(T instance)
			where T:class, new()
		{
#if UNITY_EDITOR
			if (instance == null)
			{
				GameLog.LogError("Try to return NULL value to the object pool");
				return;
			}
#endif
			if(instance is IPoolable poolable)
				poolable.Release();
			GenericPool<T>.Release(instance);
		}

		public static void Despawn<T>(ref T instance, T defaultValue = null) where T : class, new()
		{
			Despawn(instance);
			instance = defaultValue;
		}

	}
	
}