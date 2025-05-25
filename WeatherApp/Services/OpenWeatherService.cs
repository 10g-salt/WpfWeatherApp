using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using WeatherApp.Models;

namespace WeatherApp.Services;

internal class OpenWeatherService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string? ApiKey;
    private bool _disposed;

    public OpenWeatherService()
    {
        _httpClient = new HttpClient();
        ApiKey = Environment.GetEnvironmentVariable("OpenWeatherApiKey");
    }

    public async Task<WeatherInfo> GetCurrentWeatherAsync(double lon, double lat)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={ApiKey}";
        var response = await _httpClient.GetStringAsync(url);
        var weatherInfo = new WeatherInfo(response);
        return weatherInfo;
    }

    public async Task<ForecastInfo> Forecast5DayAsync(double lon, double lat)
    {
        var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={ApiKey}";
        var response = await _httpClient.GetStringAsync(url);
        var forecast = new ForecastInfo(response);
        return forecast;
    }
    public async Task<(double Latitude, double Longitude)> GetCityCoordinatesAsync(string cityname)
    {
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityname}&limit=5&appid={ApiKey}";
        var response = await _httpClient.GetStringAsync(url);
        var coords = JsonConvert.DeserializeObject<List<GeocodingInfo>>(response);

        if (coords != null && coords.Count() > 0)
        {
            var selected = coords[0];
            return (selected.Lat, selected.Lon);
        }
        else
        {
            throw new Exception("City not found");
        }
    }

    public async Task<int> GetCurrentAirQualityAsync(double lon, double lat)
    {
        var url = $"http://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={ApiKey}";
        var response = await _httpClient.GetStringAsync(url);
        
        var jObject = JObject.Parse(response);
        int aqi = jObject["list"]?[0]?["main"]?["aqi"]?.Value<int>() ?? 0;

        return aqi;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _httpClient?.Dispose();
        }

        _disposed = true;
    }

}

