namespace WeatherApp.Utils;

public static class CommonUtils
{
    public static DateTime UnixToDateTime(int unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();

        return dateTime;
    }
}
