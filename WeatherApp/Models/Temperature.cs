namespace WeatherApp.Models;

public class Temperature
{
    public double CelciusCurrent { get; }
    public double CelsiusMin { get; }
    public double CelciusMax { get; }
    public double FahrenheitCurrent { get; }
    public double FahrenheitMin { get; }
    public double FahrenheitMax { get; }
    public double KelvinCurrent { get; }
    public double KelvinMin { get; }
    public double KelvinMax { get; }

    public Temperature(double current, double min, double max)
    {
        KelvinCurrent = current;
        KelvinMin = min;
        KelvinMax = max;

        CelciusCurrent = KelvinToCelsius(current);
        CelsiusMin = KelvinToCelsius(min);
        CelciusMax = KelvinToCelsius(max);

        FahrenheitCurrent = CelsiusToFahranheit(current);
        FahrenheitMin = CelsiusToFahranheit(min);
        FahrenheitMax = CelsiusToFahranheit(max);
    }

    private static double CelsiusToFahranheit(double celsius)
    {
        return Math.Round(((9.0 / 5.0) * celsius) + 32, 3);
    }

    private static double KelvinToCelsius(double kelvin)
    {
        return Math.Round(kelvin - 273.15, 3);
    }
}
