using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Collections.ObjectModel;

namespace InternetShop;

public partial class CatalogWindow : Window
{
    private ObservableCollection<Product> _products;
    public CatalogWindow()
    {
        InitializeComponent();
        using var context = new AfanasyevContext();
        var Products = context.Products;
    }
}