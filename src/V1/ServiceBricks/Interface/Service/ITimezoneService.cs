namespace ServiceBricks
{
    public interface ITimezoneService
    {
        string GetDefaultTimezoneId();

        void ChangeUserTimezone(string timezoneId);

        TimeZoneInfo GetUserTimezoneInfo();

        List<TimeZoneInfo> GetTimezones();

        DateTimeOffset ConvertUtcToLocal(DateTimeOffset utcDateTime, string timezoneName);

        DateTimeOffset ConvertUtcToLocalForUser(DateTimeOffset utcDateTime);

        DateTime ConvertUtcToLocalForUser(DateTime utcDateTime);

        DateTimeOffset ConvertLocalToUTCForUser(DateTimeOffset localDateTime);

        DateTime ConvertLocalToUTCForUser(DateTime localDateTime);

        DateTimeOffset ConvertPostBackToUTC(DateTimeOffset serverDateTime);

        DateTime ConvertPostBackToUTC(DateTime serverDateTime);
    }
}