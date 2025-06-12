﻿    using System;

    namespace UniGame.Utils
    {
        public static class UtcTimeUtils
        {
            private static readonly DateTime StartTime = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc);

            public static long GetUtcNow()
            {
                return (long)(DateTime.UtcNow - StartTime).TotalMilliseconds;
            }
        }
    }