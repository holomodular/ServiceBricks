using Microsoft.AspNetCore.Http;

namespace ServiceBricks
{
    /// <summary>
    /// This service provides timezone conversions.
    /// </summary>
    public partial class TimezoneService : ITimezoneService
    {
        /// <summary>
        /// Claim used for user timezone.
        /// </summary>
        public const string CLAIM_TIMEZONE = "TIMEZONE";

        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected TimeZoneInfo _timeZoneInfo;

        protected static string DEFAULT_LOCAL_ID = TimeZoneInfo.Local.Id;
        protected static List<TimeZoneInfo> TIMEZONE_LIST = TimeZoneInfo.GetSystemTimeZones().ToList();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
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

        /// <summary>
        /// Get the default timezone name.
        /// </summary>
        /// <returns></returns>
        public static string DefaultTimezoneName()
        {
            return DEFAULT_LOCAL_ID;
        }

        /// <summary>
        /// Change the user timezone.
        /// </summary>
        /// <param name="timezoneId"></param>
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

        /// <summary>
        /// Get the default timezone id.
        /// </summary>
        /// <returns></returns>
        public string GetDefaultTimezoneId()
        {
            return DEFAULT_LOCAL_ID;
        }

        /// <summary>
        /// Get the timezone list.
        /// </summary>
        /// <returns></returns>
        public List<TimeZoneInfo> GetTimezones()
        {
            return TIMEZONE_LIST;
        }

        /// <summary>
        /// Get the user timezone info.
        /// </summary>
        /// <returns></returns>
        public TimeZoneInfo GetUserTimezoneInfo()
        {
            return _timeZoneInfo;
        }

        /// <summary>
        /// Convert a UTC time to a local time.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <param name="timezoneName"></param>
        /// <returns></returns>
        public DateTimeOffset ConvertUtcToLocal(DateTimeOffset utcDateTime, string timezoneName)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneName);
            return TimeZoneInfo.ConvertTime(utcDateTime, tzi);
        }

        /// <summary>
        /// Convert a UTC time to a local time.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        public DateTimeOffset ConvertUtcToLocalForUser(DateTimeOffset utcDateTime)
        {
            return TimeZoneInfo.ConvertTime(utcDateTime, _timeZoneInfo);
        }

        /// <summary>
        /// Convert a UTC time to a local time.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        public DateTime ConvertUtcToLocalForUser(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTime(utcDateTime, _timeZoneInfo);
        }

        /// <summary>
        /// Convert a local time to a UTC time.
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <returns></returns>
        public DateTimeOffset ConvertLocalToUTCForUser(DateTimeOffset localDateTime)
        {
            return TimeZoneInfo.ConvertTime(localDateTime.DateTime, _timeZoneInfo, TimeZoneInfo.Utc);
        }

        /// <summary>
        /// Convert a local time to a UTC time.
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <returns></returns>
        public DateTime ConvertLocalToUTCForUser(DateTime localDateTime)
        {
            return TimeZoneInfo.ConvertTime(localDateTime, _timeZoneInfo, TimeZoneInfo.Utc);
        }

        /// <summary>
        /// Convert a postback time to a UTC time.
        /// </summary>
        /// <param name="serverDateTime"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert a postback time to a UTC time.
        /// </summary>
        /// <param name="serverDateTime"></param>
        /// <returns></returns>
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