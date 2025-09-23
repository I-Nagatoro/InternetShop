using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop;

public partial class ViewOrderWindow : Window
{
    public int user_id;
    public int order_id;
    private List<ProductOrderList> _listProducts;
    private List<ProductOrderList> _deletedProducts;

    public ViewOrderWindow()
    {
        InitializeComponent();
        _deletedProducts = new List<ProductOrderList>();
    }

    public ViewOrderWindow(int user_id, int order_id)
    {
        InitializeComponent();
        this.user_id = user_id;
        this.order_id = order_id;
        _deletedProducts = new List<ProductOrderList>();
        LoadOrder(order_id);
    }

    public void LoadOrder(int order_id)
    {
        using var context = new User025Context();
        var order = context.Orders.FirstOrDefault(x => x.OrderId == order_id);
        if (order != null)
        {
            OrderNumber.Text = $"{order.OrderId}";
            UserNameTxt.Text = $"Имя пользователя: {context.Users.Where(x => x.UserId == order.UserId).Select(x => x.Username).FirstOrDefault()}";
            PhoneTxt.Text = context.Users.Where(x => x.UserId == order.UserId).Select(x => x.Phone).FirstOrDefault();
            AddressBox.Text = $"{order.Address}";
            DateOpen.Text = $"Дата открытия заказа: {order.StartDate}";

            if (order.EndDate != null)
            {
                DateClose.Text = $"Дата закрытия заказа: {order.EndDate}";
            }
            else
            {
                DateClose.Text = $"Дата закрытия заказа: Заказ открыт";
            }

            OrderStatus.SelectedIndex = order.Status == "Open" ? 0 : 1;
            CodePickUpTxt.Text = $"Код для получения заказа: {order.CodePickup}";
        }
        LoadProducts(order_id);
    }

    public void LoadProducts(int order_id)
    {
        using var context = new User025Context();
        var orderProducts = context.OrderProducts.Where(x => x.OrderId == order_id).ToList();
        _listProducts = new List<ProductOrderList>();

        foreach (var product in orderProducts)
        {
            _listProducts.Add(new ProductOrderList
            {
                ProductId = product.ProductsId,
                ProductName = context.Products.Where(x => x.ProductId == product.ProductsId).Select(x => x.ProductName).FirstOrDefault(),
                Count = product.Count,
                Price = context.Products.Where(x => x.ProductId == product.ProductsId).Select(x => x.Cost).FirstOrDefault()
            });
        }

        ProductsList.ItemsSource = _listProducts;
        UpdateTotalCostDisplay();
    }

    private void UpdateTotalCostDisplay()
    {
        var displayedProducts = _listProducts.Except(_deletedProducts).ToList();
        decimal totalCost = displayedProducts.Sum(p => p.Price * p.Count);
        SumCostTxt.Text = $"Общая стоимость заказа: {totalCost:C}";
    }

    public void BackClick(object sender, RoutedEventArgs e)
    {
        EditOrderWindow editOrderWindow = new EditOrderWindow(user_id);
        editOrderWindow.Show();
        Close();
    }

    public void SaveClick(object sender, RoutedEventArgs e)
    {
        using var context = new User025Context();
        var order = context.Orders.FirstOrDefault(o => o.OrderId == order_id);
        if (order != null)
        {
            order.Address = AddressBox.Text;

            var user = context.Users.FirstOrDefault(u => u.UserId == order.UserId);
            if (user != null)
            {
                user.Phone = PhoneTxt.Text;
            }

            order.Status = OrderStatus.SelectedIndex == 0 ? "Open" : "Close";

            if (order.Status == "Close")
            {
                order.EndDate=DateOnly.FromDateTime(DateTime.Now);
                DateClose.Text = $"Дата закрытия заказа: {order.EndDate}";
            }
            else
            {
                order.EndDate = null;
                DateClose.Text = $"Дата закрытия заказа: Заказ открыт";
            }

                foreach (var deletedProduct in _deletedProducts)
                {
                    var orderProduct = context.OrderProducts
                        .FirstOrDefault(op => op.OrderId == order_id && op.ProductsId == deletedProduct.ProductId);

                    if (orderProduct != null)
                    {
                        context.OrderProducts.Remove(orderProduct);
                    }
                }

            var remainingProducts = context.OrderProducts
                .Where(op => op.OrderId == order_id)
                .Join(context.Products,
                      op => op.ProductsId,
                      p => p.ProductId,
                      (op, p) => new { op.Count, p.Cost })
                .ToList();
            var displayedProducts = _listProducts.Except(_deletedProducts).ToList();
            decimal totalCost = displayedProducts.Sum(p => p.Price * p.Count);
            order.SumCost = totalCost;
            context.SaveChanges();
            _deletedProducts.Clear();
            LoadProducts(order_id);

            ErrorWindow errorWindow = new ErrorWindow("Изменения успешно сохранены");
            errorWindow.Show();
        }
    }

    public async void DeleteOrderClick(object sender, RoutedEventArgs e)
    {
        using var context = new User025Context();
        AccessOrCanselWindow aocWin = new AccessOrCanselWindow();
        var result = await ShowConfirmationDialog();
        if (result)
        {
            context.OrderProducts.Where(x => x.OrderId == order_id).ExecuteDelete();
            context.Orders.Where(x => x.OrderId == order_id).ExecuteDelete();
            context.SaveChanges();
            ErrorWindow errorWindow = new ErrorWindow("Заказ успешно удалён");
            EditOrderWindow editOrderWindow = new EditOrderWindow(user_id);
            editOrderWindow.Show();
            errorWindow.Show();
            Close();
        }
    }

    public void DeleteProductClick(object sender, RoutedEventArgs e)
    {
        var selectedProduct = ProductsList.SelectedItem as ProductOrderList;

        if (selectedProduct == null)
        {
            ErrorWindow errorWindow = new ErrorWindow("Пожалуйста, выберите продукт для удаления");
            errorWindow.Show();
            return;
        }

        _deletedProducts.Add(selectedProduct);
        var updatedList = _listProducts.Where(p => !_deletedProducts.Contains(p)).ToList();
        ProductsList.ItemsSource = updatedList;
        UpdateTotalCostDisplay();
        ErrorWindow error = new ErrorWindow("Продукт помечен для удаления. Нажмите 'Сохранить изменения' для подтверждения");
        error.Show();
    }

    private async Task<bool> ShowConfirmationDialog()
    {
        AccessOrCanselWindow aocWin = new AccessOrCanselWindow();

        aocWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        aocWin.Topmost = true;

        var result = await aocWin.ShowDialog<bool>(this);
        return result;
    }
}