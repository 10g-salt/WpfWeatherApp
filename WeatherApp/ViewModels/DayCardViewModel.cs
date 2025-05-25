using WeatherApp.Models;

namespace WeatherApp.ViewModels;

public class DayCardViewModel
{
    public string DayName { get; }
    public string IconUrl { get; }
    public string TempRange { get; }

    public DayCardViewModel(Day day)
    {
        DayName = day.DayName;
        IconUrl = $"https://openweathermap.org/img/wn/{day.Icon}@2x.png";
        TempRange = $"H: {Math.Round(day.Temperature.CelciusMax)}°  L: {Math.Round(day.Temperature.CelsiusMin)}°";
    }
}
