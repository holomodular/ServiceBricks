using Microsoft.AspNetCore.Http;

namespace ServiceBricks
{
    /// <summary>
    /// This service provides timezone conversions.
    /// </summary>
    public class TimezoneService : ITimezoneService
    {
        public const string CLAIM_TIMEZONE = "TIMEZONE";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private TimeZoneInfo _timeZoneInfo;

        private static string DEFAULT_LOCAL_ID = TimeZoneInfo.Local.Id;
        private static List<TimeZoneInfo> TIMEZONE_LIST = TimeZoneInfo.GetSystemTimeZones().ToList();

        public TimezoneService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            string timezoneName = DEFAULT_LOCAL_ID;
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user != null)
            {
                var timezoneClaim = user?.Claims?.Where(x => string.Compare(x.Type, CLAIM_TIMEZONE, true) == 0).FirstOrDefault();
                if (timezoneClaim != null)
                    timezoneName = timezoneClaim.Value;
            }
            try
            {
                _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneName);
            }
            catch
            {
                _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(DEFAULT_LOCAL_ID);
            }
        }

        public static string DefaultTimezoneName()
        {
            return DEFAULT_LOCAL_ID;
        }

        public void ChangeUserTimezone(string timezoneId)
        {
            try
            {
                _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            }
            catch
            {
                _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(DEFAULT_LOCAL_ID);
            }
        }

        public string GetDefaultTimezoneId()
        {
            return DEFAULT_LOCAL_ID;
        }

        public List<TimeZoneInfo> GetTimezones()
        {
            return TIMEZONE_LIST;
        }

        public TimeZoneInfo GetUserTimezoneInfo()
        {
            return _timeZoneInfo;
        }

        public DateTimeOffset ConvertUtcToLocal(DateTimeOffset utcDateTime, string timezoneName)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneName);
            return TimeZoneInfo.ConvertTime(utcDateTime, tzi);
        }

        public DateTimeOffset ConvertUtcToLocalForUser(DateTimeOffset utcDateTime)
        {
            return TimeZoneInfo.ConvertTime(utcDateTime, _timeZoneInfo);
        }

        public DateTime ConvertUtcToLocalForUser(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTime(utcDateTime, _timeZoneInfo);
        }

        public DateTimeOffset ConvertLocalToUTCForUser(DateTimeOffset localDateTime)
        {
            return TimeZoneInfo.ConvertTime(localDateTime.DateTime, _timeZoneInfo, TimeZoneInfo.Utc);
        }

        public DateTime ConvertLocalToUTCForUser(DateTime localDateTime)
        {
            return TimeZoneInfo.ConvertTime(localDateTime, _timeZoneInfo, TimeZoneInfo.Utc);
        }

        public DateTimeOffset ConvertPostBackToUTC(DateTimeOffset serverDateTime)
        {
            if (serverDateTime.Offset != TimeSpan.Zero)
            {
                //Do user conversion
                var useroffset = _timeZoneInfo.GetUtcOffset(serverDateTime).TotalHours; //user
                var offsetLocal = TimeZoneInfo.Local.GetUtcOffset(serverDateTime).TotalHours; //Server is different from user
                return serverDateTime.AddHours(offsetLocal - useroffset).ToUniversalTime();
            }
            return serverDateTime;
        }

        public DateTime ConvertPostBackToUTC(DateTime serverDateTime)
        {
            if (serverDateTime.Kind != DateTimeKind.Utc)
            {
                //Do user conversion
                var useroffset = _timeZoneInfo.GetUtcOffset(serverDateTime).TotalHours; //user
                var offsetLocal = TimeZoneInfo.Local.GetUtcOffset(serverDateTime).TotalHours; //Server is different from user
                return serverDateTime.AddHours(offsetLocal - useroffset).ToUniversalTime();
            }
            return serverDateTime;
        }
    }
}