using UniCore.Runtime.ProfilerTools;

namespace UniModules.UniGame.Core.Editor.Tools
{
    using System;
    using System.Diagnostics;
    using UnityEngine;
    using Debug = UnityEngine.Debug;

    public static class UniEditorProfiler
    {
        public const string TimeMessageFormat = "UniEditorProfiler: Id {0} Method {1} Time = {2} ms \n Trace : \n {3}";
        public const string Empty = "Empty";
        
        public static void LogTime(Action action)
        {
            var name = action?.Method.Name;
            LogTime(name, action);
        }
        
        public static void LogTime(string name,Action action)
        {
            if (action == null)
                return;

            name = string.IsNullOrEmpty(name) ? Empty : name;
            var methodName = action.Method.Name;
            var trace = Empty;
            
            var timer     = new Stopwatch();
            try
            {
                timer.Start();
                var startTime = timer.ElapsedMilliseconds;
                action?.Invoke();
                trace = StackTraceUtility.ExtractStackTrace();
            }
            catch (Exception e)
            {
                trace = e.StackTrace;
                throw e;
            }
            finally{
                GameLog.LogFormat(TimeMessageFormat,name,methodName,timer.ElapsedMilliseconds,trace);
                timer.Stop();
            }
        }
        
    }
}
