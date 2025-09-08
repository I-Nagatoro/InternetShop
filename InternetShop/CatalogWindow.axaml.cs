using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace InternetShop;

public partial class CatalogWindow : Window
{
    public ICommand newCommand { get; }
    public CatalogWindow()
    {
        InitializeComponent();
    }

    public CatalogWindow(string Username, int UserId)
    {
        InitializeComponent();
        LoadProducts();
    }

    public void LoadProducts()
    {
        using var context = new AfanasyevContext();
        var Products = context.Products.ToList();
        CatalogProducts.ItemsSource = Products;
    }

    public void AddToCart(object sender, RoutedEventArgs e)
    {
        var id = (int)(sender as Button)!.Tag!;
        
    }
}