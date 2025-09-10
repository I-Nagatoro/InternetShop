using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;

namespace InternetShop;

public partial class BasketWindow : Window
{
    public BasketWindow()
    {
        InitializeComponent();
    }

    public BasketWindow(int user_id)
    {
        InitializeComponent();
        using var context = new AfanasyevContext();
        var basket_id = context.Baskets.Where(x=>x.UserId==user_id).Select(x=>x.BasketId).FirstOrDefault();
        var product_id_in_basket = context.BasketProducts.Where(x => x.BasketId == basket_id).Select(x=>x.ProductId).ToList();
        List<Product_basket> product_list = new List<Product_basket>();
        foreach (var product in product_id_in_basket)
        {
            product_list.Add(new Product_basket
            {
                ProductId = product,
                Price = context.Products.Where(x => x.ProductId == product).Select(x => x.Cost).FirstOrDefault(),
                ProductName = context.Products.Where(x => x.ProductId == product).Select(x => x.ProductName).FirstOrDefault(),
                ImagePath = context.Products.Where(x => x.ProductId == product).Select(x => x.ProductImage).FirstOrDefault(),
                Count = context.BasketProducts.Where(x => x.ProductId == product).Select(x => x.Count).FirstOrDefault()
            });
        }
        BasketList.ItemsSource = product_list;
    }
}