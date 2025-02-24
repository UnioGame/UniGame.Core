namespace Game.Runtime.Tools
{
    using System;
    using System.IO;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;
    using UnityEngine.Networking;
    using Cysharp.Threading.Tasks;
    using Newtonsoft.Json;
    using UniModules;

    public static class StreamingAssetsUtils
    {
        private static string _applicationPath;

        static StreamingAssetsUtils()
        {
            _applicationPath = Application.streamingAssetsPath;
        }
        
        public static void SaveToStreamingAssets(string fileName,object value)
        {
#if UNITY_EDITOR
            var path = Application.streamingAssetsPath.CombinePath(fileName);
            var json = JsonUtility.ToJson(value, true);
            File.WriteAllText(path, json);
#endif
        }

        public static async UniTask<StreamingAssetResult<TValue>> LoadDataFromWeb<TValue>(string fileName)
        {
            var result = await LoadDataFromWeb(fileName);
            if (!result.success)
            {
                return new StreamingAssetResult<TValue>()
                {
                    success = false,
                    data = default,
                    error = result.error,
                };
            }

            var data = JsonConvert.DeserializeObject<TValue>(result.data);
            return new StreamingAssetResult<TValue>()
            {
                success = true,
                data = data,
                error = null,
            };
        }

        public static async UniTask<StreamingAssetResult> LoadDataFromWeb(string fileName)
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
                return new StreamingAssetResult 
                {
                    success = false,
                    data = null,
                    error = e
                };
            }

            var result = request.downloadHandler.text.TrimStart('\uFEFF');
            
            return new StreamingAssetResult
            {
                success = true,
                data = result,
                error = null
            };
        }
        
        public static async UniTask<StreamingAssetResult> LoadDataFromFile(string fileName)
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
                return new StreamingAssetResult 
                {
                    success = false,
                    data = null,
                    error = e
                };
            }

            var result = requestText.TrimStart('\uFEFF');
            
            return new StreamingAssetResult
            {
                success = true,
                data = result,
                error = null
            };
        }
    }
    
    public struct StreamingAssetResult<TValue>
    {
        public bool success;
        public TValue data;
        public Exception error;
    }
    
    public struct StreamingAssetResult
    {
        public bool success;
        public string data;
        public Exception error;
    }
}