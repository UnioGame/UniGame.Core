namespace UniModules.Runtime.Network
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct WebServerResult
    {
        public const string NotResponseError = "Not response";
        public static readonly WebServerResult NotResponse = new WebServerResult()
        {
            success = false,
            data = null,
            error = NotResponseError,
            responseCode = 0
        };
        
        public bool success;
        public object data;
        public string error;
        public long responseCode;
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