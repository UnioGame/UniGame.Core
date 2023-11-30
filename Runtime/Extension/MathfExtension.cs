namespace UniGame.Core.Runtime.Extension
{
	using System.Runtime.CompilerServices;
	using UnityEngine;

	public static class MathfExtension
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Abs(this float value)
		{
			return Mathf.Abs(value);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Abs(this int value)
		{
			return Mathf.Abs(value);
		}
		
		public static float Clamp(this float value, float min, float max)
		{
			return Mathf.Clamp(value, min, max);
		}
		
		public static int Clamp(this int value, int min, int max)
		{
			return Mathf.Clamp(value, min, max);
		}
	}
}