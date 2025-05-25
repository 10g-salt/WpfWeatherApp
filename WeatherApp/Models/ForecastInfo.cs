using Newtonsoft.Json.Linq;
using System.Globalization;
using WeatherApp.Utils;

namespace WeatherApp.Models;

public class ForecastInfo
{
    public int Cnt { get; }
    public List<Forecast> Forecasts { get; } = new List<Forecast>();
    public List<Day> Days { get; } = new List<Day>();

    public ForecastInfo(string jsonResponse)
    {
        var jsonData = JObject.Parse(jsonResponse);
        if(jsonData.SelectToken("cod").ToString() == "200")
        {
            Cnt = int.Parse(jsonData.SelectToken("cnt").ToString(), CultureInfo.InvariantCulture);
            foreach (JToken forecast in jsonData.SelectToken("list"))
                Forecasts.Add(new Forecast(forecast));

            ForecastByDayList();
        }

    }

    private void ForecastByDayList()
    {
        var grouped = Forecasts
            .GroupBy(f => CommonUtils.UnixToDateTime(f.DateTime).Date) // Group by calendar day
            .Take(5); // Only take 5 days

        foreach (var dayGroup in grouped)
        {
            var temps = dayGroup.Select(f => f.Main.Temperature.KelvinCurrent).ToList();
            var temperature = new Temperature(temps.Average(), temps.Min(), temps.Max());

            var lastForecast = dayGroup.Last(); // Take description/icon from last entry in the day

            string dayName = dayGroup.Key.ToString("ddd");
            string description = lastForecast.WeatherList[0].Description;
            int id = lastForecast.WeatherList[0].Id;
            string icon = lastForecast.WeatherList[0].Icon;

            Days.Add(new Day(temperature, dayName, description, id, icon));
        }
    }
}
