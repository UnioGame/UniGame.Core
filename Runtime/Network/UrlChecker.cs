namespace UniModules.Runtime.Network
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;

    public static class UrlChecker
    {
        private static WebRequestBuilder _webRequestBuilder = new();
        
        public static async UniTask<UrlCheckingResult> CheckEndPoints(string[] urls,int timeout = 5)
        {
            var urlTasks = urls.Select(x => TestUrlAsync(x,timeout));
            var testResults = await UniTask.WhenAll(urlTasks);
            var maxTime = float.MaxValue;
            var result = new UrlCheckingResult();
            
            foreach (var urlResult in testResults)
            {
                result.results.Add(urlResult);
                if (!urlResult.success) continue;
                if (urlResult.time > maxTime) continue;
                maxTime = urlResult.time;
                result.bestResult = urlResult;
            }
            
            return result;
        }
        
        public static async UniTask<UrlResult> SelectFastestEndPoint(string[] urls,int timeout = 5)
        {
            var urlTasks = urls.Select(x => TestUrlAsync(x,timeout));
            var testResults = await UniTask.WhenAll(urlTasks);
            var maxTime = float.MaxValue;
            var result = new UrlResult()
            {
                success = false,
                url = string.Empty,
            };
            
            foreach (var urlResult in testResults)
            {
                if (!urlResult.success) continue;
                if (urlResult.time > maxTime) continue;
                maxTime = urlResult.time;
                result = urlResult;
            }
            
            return result;
        }

        public static async UniTask<UrlResult> TestUrlAsync(string url,int timeout = 5)
        {
            var resultDetection = new UrlResult
            {
                url = url
            };
            
            var startTime = Time.realtimeSinceStartup;
            var result = await _webRequestBuilder.GetAsync(url,timeout:timeout);
            
            var totalTime = (Time.realtimeSinceStartup - startTime) * 1000f;
            resultDetection.time = totalTime;
            
#if UNITY_EDITOR
            GameLog.Log($"[{nameof(UrlChecker)}] URL = {url} Status = {result.success} \nWeb Error = {result.error}",
                Color.red);
#endif
            resultDetection.success = result.success;
            return resultDetection;

        }
    }
    
    [Serializable]
    public class UrlCheckingResult
    {
        public UrlResult bestResult;
        public List<UrlResult> results = new();
    }
    
    [Serializable]
    public struct UrlResult
    {
        public string url;
        public float time;
        public bool success;
    }
}