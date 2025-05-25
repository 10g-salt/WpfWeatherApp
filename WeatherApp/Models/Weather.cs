using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherApp.Models;

public class Weather
{
    public int Id { get; }
    public string Main { get; }
    public string? Description { get; }
    public string? Icon { get; }

    public Weather(JToken weatherToken)
    {
        if (weatherToken != null)
        {
            Id = int.Parse(weatherToken.SelectToken("id").ToString(), CultureInfo.InvariantCulture);
            Main = weatherToken.SelectToken("main").ToString();
            Description = weatherToken.SelectToken("description").ToString();
            Icon = weatherToken.SelectToken("icon").ToString();
        }
    }
}


