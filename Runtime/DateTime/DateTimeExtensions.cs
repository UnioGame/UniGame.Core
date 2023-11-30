﻿namespace UniModules.UniCore.Runtime.DateTime
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime UnitTime = new DateTime(1970, 1, 1);
        
        public static int ToUnixTimestamp(this DateTime date)
        {
            var unixTimestamp = (int)(date.ToUniversalTime().
                Subtract(UnitTime)).TotalSeconds;
            return unixTimestamp;
        }

        public static int GetMidnightOnUnix(this DateTime date)
        {
            var unixTimestamp = (int)(date.ToUniversalTime().
                Subtract(UnitTime)).TotalSeconds;
            var nextMidnight = ((unixTimestamp / (24 * 3600)) + 1) * (24 * 3600);
            return nextMidnight;
        }
    }
}
