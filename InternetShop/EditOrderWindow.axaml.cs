using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace InternetShop;

public partial class EditOrderWindow : Window
{
    public int user_id;
    public EditOrderWindow()
    {
        InitializeComponent();
    }

    public EditOrderWindow(int user_id)
    {
        InitializeComponent();
        this.user_id = user_id;
        LoadOrders();
    }

    public void LoadOrders()
    {
        List<OrderListModel> OrderList = new List<OrderListModel>();
        using var context = new User025Context();
        OrderList.Clear();
        var orders = context.Orders.ToList();
        foreach (var order in orders)
        {
            OrderList.Add(new OrderListModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                Username = context.Users.Where(x => x.UserId == order.UserId).Select(x => x.Username).FirstOrDefault(),
                SumCost = order.SumCost,
            });
        }
        OrdersList.ItemsSource= OrderList;
    }

    private void CheckOrderClick(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        int order_id = 0;
        if (sender is Border border)
        {
            order_id = int.Parse(border.Tag.ToString());
        }
        ViewOrderWindow viewOrderWindow = new ViewOrderWindow(user_id, order_id);
        viewOrderWindow.Show();
        Close();
    }

    public void BackClick(object sender, RoutedEventArgs e)
    {
        AdminWindow adm = new AdminWindow(user_id);
        adm.Show();
        Close();
    }
}