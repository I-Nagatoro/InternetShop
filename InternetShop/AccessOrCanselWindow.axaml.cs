using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore.Query;

namespace InternetShop;

public partial class AccessOrCanselWindow : Window
{
    public bool result { get; private set; } = false;
    public AccessOrCanselWindow()
    {
        InitializeComponent();
    }

    public void CancelClick(object sender, RoutedEventArgs e)
    {
        result = false; 
        Close(result);
    }
    public void OkClick(object sender, RoutedEventArgs e)
    {
        result = true;
        Close(result);
    }
}