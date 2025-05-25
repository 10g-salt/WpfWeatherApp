using Newtonsoft.Json;
using System.Net.Http;

namespace WeatherApp.Services;

internal class IpApiResponse
{
    public string Status { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}

internal class LocationService
{
    private static readonly HttpClient httpClient = new HttpClient();

    public async Task<(double Latitude, double Longitude)> GetCurrentLocationAsync()
    {
        var response = await httpClient.GetStringAsync("http://ip-api.com/json/");
        var location = JsonConvert.DeserializeObject<IpApiResponse>(response);

        if (location.Status == "success")
        {
            return (location.Lat, location.Lon);
        }
        else
        {
            throw new Exception("Unable to determine location.");
        }
    }
}


