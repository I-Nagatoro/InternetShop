using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace InternetShop;

public partial class ErrorWindow : Window
{
    public ErrorWindow()
    {
        InitializeComponent();
    }
    
    public ErrorWindow(string error)
    {
        InitializeComponent();
        errorMsg.Text = error;
    }

    public void CloseClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}