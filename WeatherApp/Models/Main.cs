using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherApp.Models;

public class Main
{
    public Temperature Temperature { get; }
    public double Pressure { get; }
    public double Humidity { get; }
    public double SeaLvl { get; }
    public double GroundLvl { get; }

    public Main(JToken mainToken)
    {
        if (mainToken != null)
        {
            Temperature = new Temperature(
                double.Parse(mainToken.SelectToken("temp").ToString(), CultureInfo.InvariantCulture),
                double.Parse(mainToken.SelectToken("temp_min").ToString(), CultureInfo.InvariantCulture),
                double.Parse(mainToken.SelectToken("temp_max").ToString(), CultureInfo.InvariantCulture));
            Pressure = double.Parse(mainToken.SelectToken("pressure").ToString(), CultureInfo.InvariantCulture);
            Humidity = double.Parse(mainToken.SelectToken("humidity").ToString(), CultureInfo.InvariantCulture);
            if (mainToken.SelectToken("sea_level") != null)
                SeaLvl = double.Parse(mainToken.SelectToken("sea_level").ToString(), CultureInfo.InvariantCulture);
            if (mainToken.SelectToken("grnd_level") != null)
                GroundLvl = double.Parse(mainToken.SelectToken("grnd_level").ToString(), CultureInfo.InvariantCulture);
        }
    }

}
