namespace Game.Runtime.Tools
{
    using System;
    using System.IO;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;
    using UnityEngine.Networking;
    using Cysharp.Threading.Tasks;

    public static class StreamingAssetsLoader
    {
        private static string _applicationPath;

        static StreamingAssetsLoader()
        {
            _applicationPath = Application.streamingAssetsPath;
        }
        
        public static async UniTask<StreamingAssetFromWebResult> LoadDataLikeWeb(string fileName)
        {
            var path = Path.Combine(_applicationPath, fileName);
            var request = UnityWebRequest.Get(path);

            try
            {
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                GameLog.LogError("error on web request: " + path + " " + e.Message);
                return new StreamingAssetFromWebResult 
                {
                    success = false,
                    data = null,
                    Error = e
                };
            }

            var result = request.downloadHandler.text.TrimStart('\uFEFF');
            
            return new StreamingAssetFromWebResult
            {
                success = true,
                data = result,
                Error = null
            };
        }
        
        public static async UniTask<ReadTextResult> LoadDataLikeFile(string fileName)
        {
            var path = Path.Combine(_applicationPath, fileName);
            string requestText;
            
            try
            {
                requestText = await File.ReadAllTextAsync(path);
            }
            catch (Exception e)
            {
                GameLog.LogError("error on read text result: " + path + " " + e.Message);
                return new ReadTextResult 
                {
                    success = false,
                    data = null,
                    Error = e
                };
            }

            var result = requestText.TrimStart('\uFEFF');
            
            return new ReadTextResult
            {
                success = true,
                data = result,
                Error = null
            };
        }
    }
    
    public struct ReadTextResult
    {
        public bool success;
        public string data;
        public Exception Error;
    }
    
    public struct StreamingAssetFromWebResult
    {
        public bool success;
        public string data;
        public Exception Error;
    }
}