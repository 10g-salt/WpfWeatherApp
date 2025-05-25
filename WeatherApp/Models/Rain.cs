using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherApp.Models;

public class Rain
{
    public double H1 { get; }

    public Rain(JToken rainToken)
    {
        if (rainToken != null)
            if (rainToken.SelectToken("1h") != null)
                H1 = double.Parse(rainToken.SelectToken("1h").ToString(), CultureInfo.InvariantCulture);
    }
}
