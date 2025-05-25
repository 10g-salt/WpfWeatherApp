using Newtonsoft.Json.Linq;
using System.Globalization;
using WeatherApp.Utils;

namespace WeatherApp.Models;

public class Sys
{
    public int Type { get; }
    public int Id { get; }
    public string? Country { get; }
    public DateTime Sunrise { get; }
    public DateTime Sunset { get; }

    public Sys(JToken sysToken)
    {
        if (sysToken != null)
        {
            if (sysToken.SelectToken("type") != null)
                Type = int.Parse(sysToken.SelectToken("type").ToString(), CultureInfo.InvariantCulture);
            if (sysToken.SelectToken("id") != null)
                Id = int.Parse(sysToken.SelectToken("id").ToString(), CultureInfo.InvariantCulture);
            Country = sysToken.SelectToken("country").ToString();
            Sunrise = CommonUtils.UnixToDateTime(int.Parse(sysToken.SelectToken("sunrise").ToString(), CultureInfo.InvariantCulture));
            Sunset = CommonUtils.UnixToDateTime(int.Parse(sysToken.SelectToken("sunset").ToString(), CultureInfo.InvariantCulture));
        }
    }
}
