using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using InternetShop.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;

namespace InternetShop;

public partial class ProductWindow : Window
{
    public int product_id;
    public ProductWindow()
    {
        InitializeComponent();
    }
    public ProductWindow(int productId)
    {
        InitializeComponent();
        product_id = productId;
    }
    public void DoneProductClick(object sender, RoutedEventArgs e)
    {
        using var context = new User025Context();
        var product_name = ProductNameBox.Text;
        var product_caption = CaptionBox.Text;
        var product_cost = CostBox.Text;
        var ImagePath = ProductImage.Source.ToString();

        if (product_id != null && context.Products.Where(x => x.ProductId == product_id).Any())
        {
            if (Validation(product_name,product_caption,product_cost))
            {

            }
        }
        else
        {
            context.Products.Add(new Product
            {
                ProductId = context.Products.Max(x => x.ProductId) + 1,
                ProductName = product_name,
                ProductCaption = product_caption,
                Cost = decimal.Parse(product_cost),
                // ProductImage = 
            });
        }
    }

    public bool Validation(string product_name, string product_caption, string product_cost)
    {
        decimal productCost = 0;
        if (product_name == null || product_caption == null || product_cost == null)
        {
            ErrTxt.Text = "Заполните все поля";
            return false;
        }else if (decimal.TryParse(product_cost, out productCost))
        {
            ErrTxt.Text = "Неверное значение цены";
            return false;
        }
        return true;
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });
        var path = files[0].Path.LocalPath;
        var guid = Guid.NewGuid().ToString("N");
        File.Copy(path, AppDomain.CurrentDomain.BaseDirectory + "/image_products/" + guid);
        var pathDB = $"image_products/{guid}";
    }
}