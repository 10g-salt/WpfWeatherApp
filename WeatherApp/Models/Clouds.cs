using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherApp.Models;

public class Clouds
{
    public double All { get; }

    public Clouds(JToken cloudToken)
    {
        if (cloudToken != null)
            All = double.Parse(cloudToken.SelectToken("all").ToString(), CultureInfo.InvariantCulture);

    }
}
