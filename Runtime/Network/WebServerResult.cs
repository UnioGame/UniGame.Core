namespace UniModules.Runtime.Network
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct WebServerResult
    {
        public bool success;
        public object data;
        public string error;
    }

    [Serializable]
    public struct WebServerTexture2DResult
    {
        public Texture2D texture;
        public bool success;
        public string error;
    }
    
    [Serializable]
    public struct WebServerSpriteResult
    {
        public Sprite sprite;
        public bool success;
        public string error;
    }
}