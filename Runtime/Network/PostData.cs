namespace UniModules.Runtime.Network
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public struct PostData
    {
        public static readonly PostData Empty = new PostData()
        {
            data = Array.Empty<byte>(),
            contentType = string.Empty
        };

        public string url;
        public Dictionary<string, string> form;
        public Dictionary<string, string> headers;
        public Dictionary<string, string> parameters;
        
        public byte[] data;
        public string contentType;
    }
}