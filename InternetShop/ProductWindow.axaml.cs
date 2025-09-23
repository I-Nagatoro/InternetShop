using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using InternetShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;

namespace InternetShop;

public partial class ProductWindow : Window
{
    public int product_id;
    public int user_id;
    public string product_image;
    public string partImagedb;
    public ProductWindow()
    {
        InitializeComponent();
        ProductBox.SelectedItem = null;
    }
    public ProductWindow(int UpdateOrAdd, int userId)
    {
        InitializeComponent();
        ProductBox.SelectedItem = null;
        if (UpdateOrAdd == 0)
        {
            EditTxt.IsVisible = true;
            ProductBox.IsVisible = true;
        }
        user_id = userId;
        LoadProductList();
    }

    public void LoadProductList()
    {
        using var context = new User025Context();
        var products = context.Products.Select(x=>x.ProductName).ToList();
        ProductBox.ItemsSource = products;
    }
    public void DoneProductClick(object sender, RoutedEventArgs e)
    {
        using var context = new User025Context();
        if (!ProductBox.IsVisible)
        {
            var product_name = ProductNameBox.Text;
            var product_caption = CaptionBox.Text;
            var product_cost = CostBox.Text;

            if (Validation(product_name, product_caption, product_cost))
            {
                if (product_image == null)
                {
                    product_image = "image_products/picture.png";
                }

                context.Products.Add(new Product
                {
                    ProductName = product_name,
                    ProductCaption = product_caption,
                    Cost = decimal.Parse(product_cost),
                    ProductImage = product_image
                });

                context.SaveChanges();

                ErrorWindow errorWindow = new ErrorWindow("Продукт успешно добавлен!");
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                errorWindow.Show();
                Close();
            }
        }
        else
        {
            var product_id = context.Products.Where(x=>x.ProductName==ProductBox.SelectedItem).Select(x=>x.ProductId).FirstOrDefault();
            var MainProduct=context.Products.Where(x=>x.ProductId==product_id).FirstOrDefault();
            var product_name = ProductNameBox.Text;
            var product_caption = CaptionBox.Text;
            var product_cost = CostBox.Text;
            ProductBox.SelectedItem = null;

            if (Validation(product_name, product_caption, product_cost))
            {
                if (product_image == null)
                {
                    product_image = "image_products/picture.png";
                }
                

                MainProduct.ProductName=product_name;
                MainProduct.ProductCaption=product_caption;
                MainProduct.ProductImage= product_image;
                MainProduct.Cost=decimal.Parse(product_cost);

                context.SaveChanges();

                ErrorWindow errorWindow = new ErrorWindow("Продукт успешно обновлён!");
                AdminWindow adminWindow = new AdminWindow(user_id);
                adminWindow.Show();
                errorWindow.Show();
                Close();
            }
        }
    }

    public bool Validation(string product_name, string product_caption, string product_cost)
    {
        decimal productCost = 0;

        if (string.IsNullOrEmpty(product_name) || string.IsNullOrEmpty(product_caption) || string.IsNullOrEmpty(product_cost))
        {
            ErrTxt.Text = "Заполните все поля";
            return false;
        }
        else if (!decimal.TryParse(product_cost, out productCost) || productCost <= 0)
        {
            ErrTxt.Text = "Неверное значение цены";
            return false;
        }
        return true;
    }

    private async void AddImageClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });
        if (files.Any())
        {
            var path = files[0].Path.LocalPath;
            var guid = Guid.NewGuid().ToString("N");
            var image_path = path.Split(@"\").Last();
            File.Copy(path, AppDomain.CurrentDomain.BaseDirectory + "/image_products/" + guid);
            product_image = $"image_products/{guid}";
            ProductImage.Source = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/image_products/" + guid);
        }
    }

    private void Product_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ProductBox.SelectedItem == null) return;

        using var context = new User025Context();
        var product_name = ProductBox.SelectedItem as string;
        var product = context.Products.FirstOrDefault(x => x.ProductName == product_name);

        if (product != null)
        {
            ProductNameBox.Text = product.ProductName;
            CaptionBox.Text = product.ProductCaption;
            CostBox.Text = product.Cost.ToString();

            if (!string.IsNullOrEmpty(product.ProductImage))
            {
                product_image = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, product.ProductImage);
                if (File.Exists(product_image))
                {
                    ProductImage.Source = new Bitmap(product_image);
                }
                else
                {
                    ProductImage.Source = new Bitmap(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image_products/picture.png"));
                }
                product_image = product.ProductImage;
            }
        }
    }

    public void BackClick(object? sender, RoutedEventArgs e)
    {
        AdminWindow adm = new AdminWindow(user_id);
        adm.Show();
        Close();
    }
}