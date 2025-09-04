using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace InternetShop;

public partial class CatalogWindow : Window
{
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
}