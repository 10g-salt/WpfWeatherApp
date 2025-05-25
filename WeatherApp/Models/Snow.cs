using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherApp.Models;

public class Snow
{
    public double H1 { get; }

    public Snow(JToken snowToken)
    {
        if (snowToken != null)
            if (snowToken.SelectToken("1h") != null)
                H1 = double.Parse(snowToken.SelectToken("1h").ToString(), CultureInfo.InvariantCulture);
    }
}
