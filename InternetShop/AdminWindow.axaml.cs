using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace InternetShop;

public partial class AdminWindow : Window
{
    public AdminWindow()
    {
        InitializeComponent();
    }

    public void AddProductClick(object sender, RoutedEventArgs e)
    {
        ProductWindow productWindow = new ProductWindow();
        productWindow.Show();
        Close();
    }
    public void UpdateProductClick(object sender, RoutedEventArgs e)
    {

    }
    public void DeleteProductClick(object sender, RoutedEventArgs e)
    {

    }
    public void ShowUpdateOrdersClick(object sender, RoutedEventArgs e)
    {

    }
}