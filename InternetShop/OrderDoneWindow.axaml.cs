using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Linq;

namespace InternetShop;

public partial class OrderDoneWindow : Window
{
    public OrderDoneWindow()
    {
        InitializeComponent();
    }

    public OrderDoneWindow(int user_id)
    {
        InitializeComponent();
        using var context = new User025Context();
        var code = context.Orders.Where(x=>x.UserId==user_id).Select(x=>x.CodePickup).First();
        CodeTxt.Text = $"Код для получения заказа: {code}";
    }
}