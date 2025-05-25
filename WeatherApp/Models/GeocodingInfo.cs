using Newtonsoft.Json;

namespace WeatherApp.Models;

public class GeocodingInfo
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("country")]
    public string? Country { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }
}
