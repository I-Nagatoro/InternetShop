using Avalonia;
using Avalonia.Controls;
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
}