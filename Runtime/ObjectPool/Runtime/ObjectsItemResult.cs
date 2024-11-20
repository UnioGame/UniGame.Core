namespace UniGame.Runtime.ObjectPool
{
    using System;

    [Serializable]
    public struct ObjectsItemResult<T>
    {
        public static readonly ObjectsItemResult<T> Empty = new ObjectsItemResult<T>()
        {
            First = default,
            Items = Array.Empty<T>(),
            Success = false,
        };
        
        public static readonly ObjectsItemResult<T> Single = new ObjectsItemResult<T>()
        {
            First = default,
            Items = Array.Empty<T>(),
            Success = true,
        };
        
        public T First;
        public T[] Items;
        public bool Success;
        public int Length;
    }
}