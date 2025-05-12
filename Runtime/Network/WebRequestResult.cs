namespace UniModules.Runtime.Network
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct WebRequestResult
    {
        public const string NotResponseError = "Not response";
        public static readonly WebRequestResult NotResponse = new WebRequestResult()
        {
            success = false,
            data = null,
            error = NotResponseError,
            responseCode = 0
        };
        
        public string url;
        public bool success;
        public bool networkError;
        public bool httpError;
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