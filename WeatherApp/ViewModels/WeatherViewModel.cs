using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Services;
using WeatherApp.Utils;

namespace WeatherApp.ViewModels;

public class WeatherViewModel : INotifyPropertyChanged
{
    private readonly OpenWeatherService _weatherService = new();
    private readonly LocationService _locationService = new();

    private string _searchCity;
    public string SearchCity
    {
        get => _searchCity;
        set => SetProperty(ref _searchCity, value);
    }

    private WeatherInfo _currentWeather;
    public WeatherInfo CurrentWeather
    {
        get => _currentWeather;
        set
        {
            if (SetProperty(ref _currentWeather, value))
            {
                OnPropertyChanged(nameof(Temperature));
                OnPropertyChanged(nameof(Condition));
                OnPropertyChanged(nameof(DisplayCity));
                OnPropertyChanged(nameof(WeatherIconUrl));
                OnPropertyChanged(nameof(DayAndTime));
                OnPropertyChanged(nameof(Humidity));
                OnPropertyChanged(nameof(Wind));
                OnPropertyChanged(nameof(Visibility));
                OnPropertyChanged(nameof(Rain));
                OnPropertyChanged(nameof(Sunrise));
                OnPropertyChanged(nameof(Sunset));
            }
        }
    }
    public ObservableCollection<DayCardViewModel> DailyForecasts { get; } = [];

    private ForecastInfo _forecastWeather;
    public ForecastInfo ForecastWeather
    {
        get => _forecastWeather;
        set
        {
            if (SetProperty(ref _forecastWeather, value))
            {
                DailyForecasts.Clear();
                foreach (var day in _forecastWeather.Days)
                    DailyForecasts.Add(new DayCardViewModel(day));
            }
        }
    }

    private int _airQuality;
    public int AirQuality
    {
        get => _airQuality;
        set 
        {
            if (SetProperty(ref _airQuality, value))
                OnPropertyChanged(nameof(AirQualityDescription));
        }
    }

    private bool _isUsingCurrentLocation;
    public bool IsUsingCurrentLocation
    {
        get => _isUsingCurrentLocation;
        set => SetProperty(ref _isUsingCurrentLocation, value);
    }



    // Derived properties
    public string DisplayCity => CurrentWeather?.Name ?? SearchCity;
    public string Temperature => $"{Math.Round(CurrentWeather?.Main?.Temperature?.CelciusCurrent ?? 0)} °C";
    public string DayAndTime =>
        DateTimeOffset.FromUnixTimeSeconds(CurrentWeather?.DateTime ?? 0)
            .LocalDateTime
            .ToString("dddd HH:mm");
    public string Condition => CurrentWeather?.WeatherList?[0]?.Description ?? "Unknown";
    public string? WeatherIconUrl =>
        CurrentWeather?.WeatherList?[0]?.Icon is string iconCode
            ? $"https://openweathermap.org/img/wn/{iconCode}@2x.png"
            : null;
    public string AirQualityDescription =>
        AirQuality switch
        {
            1 => "Good",
            2 => "Fair",
            3 => "Moderate",
            4 => "Poor",
            5 => "Very Poor",
            _ => "Unknown"
        };
    public string Humidity => $"{CurrentWeather?.Main?.Humidity ?? 0}";
    public string Wind => $"{Math.Round(CurrentWeather?.Wind?.Speed ?? 0.0,2)}";
    public string Visibility => $"{(CurrentWeather?.Visibility)/1000 ?? 0.0}";
    public string Rain => $"{CurrentWeather?.Rain?.H1 ?? 0.0}";
    public string Sunrise =>
        CurrentWeather?.Sys?.Sunrise.ToString("HH:mm") ?? "unknown";
    public string Sunset =>
        CurrentWeather?.Sys?.Sunset.ToString("HH:mm") ?? "unknwon";


    public ICommand FetchByCityCommand { get; }
    public ICommand ResetToCurrentLocationCommand { get; }

    public WeatherViewModel()
    {
        System.Diagnostics.Debug.WriteLine("WeatherViewModel loaded");
        FetchByCityCommand = new RelayCommand(async () => await LoadWeatherByCityAsync());
        ResetToCurrentLocationCommand = new RelayCommand(async () => await LoadWeatherByLocationAsync());

        // Auto-load on creation 
        _ = LoadWeatherByLocationAsync();
    }

    private async Task LoadWeatherByLocationAsync()
    {
        try
        {
            IsUsingCurrentLocation = true;
            var (lat, lon) = await _locationService.GetCurrentLocationAsync();
            CurrentWeather = await _weatherService.GetCurrentWeatherAsync(lon, lat);
            ForecastWeather = await _weatherService.Forecast5DayAsync(lon, lat);
            AirQuality = await _weatherService.GetCurrentAirQualityAsync(lon, lat);
        }
        catch (Exception ex)
        {
            if (Application.Current == null || Application.Current.MainWindow == null || !Application.Current.MainWindow.IsLoaded)
                return;

            System.Diagnostics.Debug.WriteLine($"[Location Weather Load Error] {ex}");
            System.Windows.MessageBox.Show(
                "Failed to load weather using current location. Please check your internet or location settings.",
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error
            );
        }
    }

    public async Task LoadWeatherByCityAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchCity))
            return;

        try
        {
            IsUsingCurrentLocation = false;
            var (lat, lon) = await _weatherService.GetCityCoordinatesAsync(SearchCity);
            CurrentWeather = await _weatherService.GetCurrentWeatherAsync(lon, lat);
            ForecastWeather = await _weatherService.Forecast5DayAsync(lon, lat);
            AirQuality = await _weatherService.GetCurrentAirQualityAsync(lon, lat);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[City Weather Load Error] {ex}");
            System.Windows.MessageBox.Show(
                $"Failed to load weather for '{SearchCity}'. Please check the city name or your connection.",
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning
            );
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
