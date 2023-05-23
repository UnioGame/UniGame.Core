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
	}
}