using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeatherApp.ViewModels;

namespace WeatherApp.Views;

/// <summary>
/// Interaction logic for WeatherView.xaml
/// </summary>
public partial class WeatherView : UserControl
{
    public WeatherView()
    {
        InitializeComponent();
        this.DataContext = new WeatherViewModel();
    }

    private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var textBox = sender as TextBox;
            textBox?.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            if (DataContext is WeatherViewModel vm)
            {
                await vm.LoadWeatherByCityAsync(); // Call the method directly
            }
        }
    }
}
