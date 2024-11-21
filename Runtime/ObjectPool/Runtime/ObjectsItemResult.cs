namespace UniGame.Runtime.ObjectPool
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct ObjectsItemResult
    {
        public static readonly ObjectsItemResult Empty = new()
        {
            First = default,
            Items = Array.Empty<GameObject>(),
            Success = false,
        };
        
        public static readonly ObjectsItemResult Single = new()
        {
            First = default,
            Items = Array.Empty<GameObject>(),
            Success = true,
        };
        
        public GameObject First;
        public GameObject[] Items;
        public bool Success;
        public int Length;
    }
}