namespace WeatherApp.Models;

public class Day
{
    public Temperature Temperature { get; }
    public string DayName { get; }
    public string Description { get; }
    public int Id { get; }
    public string Icon { get; }

    public Day(Temperature temperature, string dayName, string description, int id, string icon)
    {
        Temperature = temperature;
        DayName = dayName;
        Description = description;
        Id = id;
        Icon = icon;
    }
}
