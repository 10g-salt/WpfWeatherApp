using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherApp.Models;

public class Coordinates
{
    public double Lon {  get; }
    public double Lat { get; }

    public Coordinates(JToken coordinateToken)
    {
        if (coordinateToken != null)
        {
            Lon = double.Parse(coordinateToken.SelectToken("lon").ToString(), CultureInfo.InvariantCulture);
            Lat = double.Parse(coordinateToken.SelectToken("lat").ToString(), CultureInfo.InvariantCulture);
        }
    }
}
