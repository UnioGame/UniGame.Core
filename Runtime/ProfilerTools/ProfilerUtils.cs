using UniModules.UniCore.Runtime.Utils;

namespace UniModules.UniCore.Runtime.ProfilerTools
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class ProfilerUtils
    {

        public static MemorizeItem<string, Stopwatch> stopWatchSource = MemorizeTool.Memorize<string, Stopwatch>(x => new Stopwatch());

        public static long GetMemorySize(object target)
        {

            long size = 0;
            using (var s = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                try {
                    formatter.Serialize(s, target);
                }
                catch (Exception) {}
                size = s.Length;
            }

            return size;

        }

        public static ProfilerId BeginWatch(string message)
        {
            var profilerId = new ProfilerId()
            {
                id = System.Guid.NewGuid().ToString(),
                message = message,
            };

            var stopWatch = stopWatchSource[profilerId.id];
            stopWatch.Reset();
            stopWatch.Start();

            return profilerId;
        }
        
        public static WatchProfileResult GetWatchData(ProfilerId profilerId,bool reset = false)
        {
            var stopWatch = stopWatchSource[profilerId.id];
            var result = new WatchProfileResult()
            {
                watchMs = stopWatch.ElapsedMilliseconds,
            };

            if(reset) stopWatch.Reset();
            
            return result;
        }
        
        public static WatchProfileResult StopWatch(ProfilerId profilerId)
        {
            return GetWatchData(profilerId, true);
        }

    }

    [Serializable]
    public struct ProfilerId
    {
        public string id;
        public string message;
    }
    
    [Serializable]
    public struct WatchProfileResult
    {
        public long watchMs;
    }
}
