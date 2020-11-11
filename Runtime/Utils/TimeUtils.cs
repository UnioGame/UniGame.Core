    using System;

    namespace UniModules.UniGame.Core.Runtime.Utils
    {
        public static class TimeUtils
        {
            private static readonly DateTime StartTime = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc);

            public static long GetUtcNow()
            {
                return (long)(DateTime.UtcNow - StartTime).TotalMilliseconds;
            }
        }
    }