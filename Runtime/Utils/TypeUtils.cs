namespace UniGame.Runtime.Utils {
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;

	public static class TypeUtils
	{
		private static MemorizeItem<string, uint> _hashCache = MemorizeTool.Memorize<string, uint>(GetJavaHash);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint GetJavaHash(string s) {

			var hash = 0u;
			for (var i = 0; i < s.Length; i++) {
				var c = s[i];
				hash = 31u * hash + c;
			}
			return hash;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<long> GetTypeIds(this System.Type type) {

			var typeList = new List<long>();
			var objectType = typeof(object);
			while (type != objectType) {
				typeList.Add(type.GetTypeId());
				type = type.BaseType;
			}

			return typeList;

		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<string> GetTypeNames(this System.Type type) {

			var typeList   = new List<string>();
			var objectType = typeof(object);
			while (type != objectType) {
				typeList.Add(type.AssemblyQualifiedName);
				type = type.BaseType;
			}

			return typeList;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint GetTypeId(this System.Type type)
		{
			return _hashCache[type.FullName];
		}

	}
}
