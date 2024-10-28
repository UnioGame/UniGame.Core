namespace UniModules.Runtime.Network
{
    using System;

    [Serializable]
    public struct PostData
    {
        public static readonly PostData Empty = new PostData()
        {
            data = Array.Empty<byte>(),
            contentType = string.Empty
        };
            
        public byte[] data;
        public string contentType;
    }
}