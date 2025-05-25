using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherApp.Models;

public class Forecast
{
    public int DateTime { get; }
    public Main? Main { get; }
    public List<Weather> WeatherList { get; } = new List<Weather>();
    public double RainPossibility { get; }
    
    public Forecast(JToken forecastToken)
    {
        if (forecastToken != null)
        {
            DateTime = int.Parse(forecastToken.SelectToken("dt").ToString(), CultureInfo.InvariantCulture);
            Main = new Main(forecastToken.SelectToken("main"));
            foreach (JToken weather in forecastToken.SelectToken("weather"))
                WeatherList.Add(new Weather(weather));
        }
    }
}

