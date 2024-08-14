namespace ServiceBricks
{
    public partial interface ITimezoneService
    {
        /// <summary>
        /// Get the default timezone ID.
        /// </summary>
        /// <returns></returns>
        string GetDefaultTimezoneId();

        /// <summary>
        /// Change the user timezone.
        /// </summary>
        /// <param name="timezoneId"></param>
        void ChangeUserTimezone(string timezoneId);

        /// <summary>
        /// Get the user timezone info.
        /// </summary>
        /// <returns></returns>
        TimeZoneInfo GetUserTimezoneInfo();

        /// <summary>
        /// Get the timezones.
        /// </summary>
        /// <returns></returns>
        List<TimeZoneInfo> GetTimezones();

        /// <summary>
        /// Convert the UTC time to the local time.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <param name="timezoneName"></param>
        /// <returns></returns>
        DateTimeOffset ConvertUtcToLocal(DateTimeOffset utcDateTime, string timezoneName);

        /// <summary>
        /// Convert the UTC time to the local time.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        DateTimeOffset ConvertUtcToLocalForUser(DateTimeOffset utcDateTime);

        /// <summary>
        /// Convert the UTC time to the local time.
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        DateTime ConvertUtcToLocalForUser(DateTime utcDateTime);

        /// <summary>
        /// Convert the local time to the UTC time.
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <returns></returns>
        DateTimeOffset ConvertLocalToUTCForUser(DateTimeOffset localDateTime);

        /// <summary>
        /// Convert the local time to the UTC time.
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <returns></returns>
        DateTime ConvertLocalToUTCForUser(DateTime localDateTime);

        /// <summary>
        /// Convert the postback time to the UTC time.
        /// </summary>
        /// <param name="serverDateTime"></param>
        /// <returns></returns>
        DateTimeOffset ConvertPostBackToUTC(DateTimeOffset serverDateTime);

        /// <summary>
        /// Convert the postback time to the UTC time.
        /// </summary>
        /// <param name="serverDateTime"></param>
        /// <returns></returns>
        DateTime ConvertPostBackToUTC(DateTime serverDateTime);
    }
}