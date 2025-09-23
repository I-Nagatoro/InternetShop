using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop;

public partial class DeleteProductWindow : Window
{
    public int user_id;
    public DeleteProductWindow()
    {
        InitializeComponent();
    }

    public DeleteProductWindow(int user_id)
    {
        InitializeComponent();
        this.user_id = user_id;
        LoadProducts();
    }

    public void LoadProducts()
    {
        using var context = new User025Context();
        var Products = context.Products.Select(x=>x.ProductName).ToList();
        ProductCombo.ItemsSource=Products;
    }

    public async void DeleteClick(object sender, RoutedEventArgs e)
    {
        using var context = new User025Context();
        if (ProductCombo.SelectedItem != null)
        {
            AccessOrCanselWindow aocWin = new AccessOrCanselWindow();
            var result = await ShowConfirmationDialog();
            if (result)
            {
                var product_name = ProductCombo.SelectedItem as string;
                var product_id = context.Products.Where(x => x.ProductName == product_name).Select(x => x.ProductId).FirstOrDefault();
                if (product_id != null)
                {
                    var order_id = context.OrderProducts.Where(x => x.ProductsId == product_id).Select(x => x.OrderId).FirstOrDefault();
                    context.OrderProducts.Where(x => x.ProductsId == product_id).ExecuteDelete();
                    if (!context.OrderProducts.Where(x => x.OrderId == order_id).Any())
                    {
                        context.Orders.Where(x => x.OrderId == order_id).ExecuteDelete();
                    }
                    context.BasketProducts.Where(x => x.ProductId == product_id).ExecuteDelete();
                    context.Products.Where(x => x.ProductId == product_id).ExecuteDelete();
                    context.SaveChanges();
                }
                ErrorWindow errorWindow = new ErrorWindow("Продукт успешно удалён!");
                errorWindow.Show();
                LoadProducts();
            }
        }
        else
        {
            ErrorWindow errorWindow = new ErrorWindow("Выберите продукт");
            errorWindow.Show();
        }
    }

    private async Task<bool> ShowConfirmationDialog()
    {
        AccessOrCanselWindow aocWin = new AccessOrCanselWindow();

        aocWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        aocWin.Topmost = true;

        var result = await aocWin.ShowDialog<bool>(this);
        return result;
    }

    public void BackClick(object sender, RoutedEventArgs e)
    {
        AdminWindow adminWindow = new AdminWindow(user_id);
        adminWindow.Show();
        Close();
    }
}