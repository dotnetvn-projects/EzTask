using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Common
{
    public static class DateTimeUtilities
    {
        /// <summary>
        /// Convert dd/mm/yyy to datatime
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime? ParseFromString(string data)
        {
            if(string.IsNullOrEmpty(data))
            {
                return null; 
            }
            var date = data.Split('/');
            return new DateTime(int.Parse(date[2]), int.Parse(date[1]),int.Parse(date[0]));
        }

        public static string TimeAgo(this DateTime dateTime)
        {
            TimeSpan span = DateTime.Now - dateTime;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }

            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }

            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");

            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");

            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");

            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);

            if (span.Seconds <= 5)
                return "just now";

            return string.Empty;
        }
    }
}
