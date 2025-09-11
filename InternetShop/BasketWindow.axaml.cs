using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;

namespace InternetShop;

public partial class BasketWindow : Window
{
    public int userId;
    public BasketWindow()
    {
        InitializeComponent();
    }

    public BasketWindow(int user_id)
    {
        InitializeComponent();
        userId = user_id;
        LoadProducts();
    }

    public void LoadProducts()
    {
        int user_id = userId;
        decimal SumPrice = 0;
        using var context = new AfanasyevContext();
        var basket_id = context.Baskets.Where(x => x.UserId == user_id).Select(x => x.BasketId).FirstOrDefault();
        var product_id_in_basket = context.BasketProducts.Where(x => x.BasketId == basket_id).Select(x => x.ProductId).ToList().Order();
        List<Product_basket> product_list = new List<Product_basket>();
        foreach (var product in product_id_in_basket)
        {
            product_list.Add(new Product_basket
            {
                ProductId = product,
                Price = context.Products.Where(x => x.ProductId == product).Select(x => x.Cost).FirstOrDefault(),
                ProductName = context.Products.Where(x => x.ProductId == product).Select(x => x.ProductName).FirstOrDefault(),
                ImagePath = context.Products.Where(x => x.ProductId == product).Select(x => x.ProductImage).FirstOrDefault(),
                Count = context.BasketProducts.Where(x => x.ProductId == product).Select(x => x.Count).FirstOrDefault(),
                Caption = context.Products.Where(x => x.ProductId == product).Select(x => x.ProductCaption).FirstOrDefault()
            });
            SumPrice += context.Products.Where(x => x.ProductId == product).Select(x => x.Cost).FirstOrDefault() * context.BasketProducts.Where(x => x.ProductId == product).Select(x => x.Count).FirstOrDefault();
        }
        product_list.OrderBy(x=>x.Caption);
        BasketList.ItemsSource = product_list;
        sumprice.Text=SumPrice.ToString();
    }

    public void DecreaseQuantity(object sender, RoutedEventArgs e)
    {
        using var context = new AfanasyevContext();
        Button btnMin = sender as Button;
        var product_id = (int)btnMin.Tag;
        var product = context.BasketProducts.FirstOrDefault(x=>x.ProductId==product_id);
        if (product != null)
        {
            if (product.Count > 1)
            {
                product.Count--;
                context.SaveChanges();
            }
            else
            {
                context.BasketProducts.Remove(product);
                context.SaveChanges();
            }
            LoadProducts();
        }
    }
    public void IncreaseQuantity(object sender, RoutedEventArgs e)
    {
        using var context = new AfanasyevContext();
        Button btnMin = sender as Button;
        var product_id = (int)btnMin.Tag;
        var product = context.BasketProducts.FirstOrDefault(x => x.ProductId == product_id);
        if (product != null)
        {
            product.Count++;
            context.SaveChanges();
            LoadProducts();
        }
    }
    public void Back(object sender, RoutedEventArgs e)
    {
        CatalogWindow Catalog = new CatalogWindow(userId);
        Catalog.Show();
        Close();
    }

    public void OrderDone(object sender, RoutedEventArgs e)
    {

    }
}