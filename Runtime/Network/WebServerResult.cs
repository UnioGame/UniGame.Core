namespace UniModules.Runtime.Network
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct WebServerResult
    {
        public bool success;
        public string data;
        public string error;
        public Exception exception;
    }

    [Serializable]
    public struct WebServerTexture2DResult
    {
        public Texture2D texture;
        public bool success;
        public string error;
        public Exception exception;
    }
    
    [Serializable]
    public struct WebServerSpriteResult
    {
        public Sprite sprite;
        public bool success;
        public string error;
        public Exception exception;
    }
}