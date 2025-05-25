using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherApp.Models;

public class Wind
{
    public double Speed { get; } // meters per sec

    public Wind(JToken windToken)
    {
        if (windToken != null)
        {
            Speed = double.Parse(windToken.SelectToken("speed").ToString(), CultureInfo.InvariantCulture) * 3.6;
        }
    }
}
