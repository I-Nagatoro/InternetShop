using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternetShop;

public partial class OrderWindow : Window
{
    public int user_id;

    public List<Product_basket> products;
    public OrderWindow()
    {
        InitializeComponent();
    }

    public OrderWindow(int userId, List<Product_basket> product_list)
    {
        InitializeComponent();
        using var context = new User025Context();
        LoadOrderDetail(product_list);
        string username = context.Users.Where(x=>x.UserId==userId).Select(x=>x.Username).FirstOrDefault();
        usernameTxt.Text = username;
        user_id = userId;
        products = product_list;
    }

    public void LoadOrderDetail(List<Product_basket> product_list)
    {
        productOrderList.ItemsSource=product_list;
    }

    public void BackClick(object sender, RoutedEventArgs e)
    {
        BasketWindow backet = new BasketWindow(user_id);
        backet.Show();
        this.Close();
    }

    public void OrderDoneClick(object sender, RoutedEventArgs e)
    {
        using var context = new User025Context();
        var order_id = context.Orders.Select(x => x.OrderId).Any() ? context.Orders.Max(x => x.OrderId) + 1 : 1;
        var username = usernameTxt.Text;
        var address = addressTxt.Text;
        var phone = phoneTxt.Text;
        var code_pickup = Guid.NewGuid().ToString("D").ToUpper().Split('-')[0];

        decimal sumcost = 0;

        foreach (var product in products)
        {
            context.OrderProducts.Add(new OrderProduct
            {
                OrderId = order_id,
                Count = product.Count,
                ProductsId = product.ProductId
            });
            sumcost += product.Count * product.Price;
        }

        context.Orders.Add(new Order
        {
            OrderId = order_id,
            UserId = user_id,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            Status = "Open",
            Address = address,
            Phone=phone,
            SumCost = sumcost,
            CodePickup = code_pickup
        });
        context.BasketProducts.Where(x=>x.BasketId==context.Baskets.Where(u=>u.UserId==user_id).Select(i=>i.BasketId).FirstOrDefault()).ExecuteDelete();
        context.SaveChanges();
        CatalogWindow catalog = new CatalogWindow(user_id);
        OrderDoneWindow done = new OrderDoneWindow(user_id);
        catalog.Show();
        done.Show();
        this.Close();
    }
}