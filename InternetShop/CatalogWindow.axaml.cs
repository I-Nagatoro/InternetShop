using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace InternetShop;

public partial class CatalogWindow : Window
{
    public ICommand newCommand { get; }
    public int Basket_id;
    public int user_id;
    public CatalogWindow()
    {
        InitializeComponent();
    }

    public CatalogWindow(string Username, int UserId)
    {
        InitializeComponent();
        LoadProducts();
        using var context = new AfanasyevContext();
        if (context.Baskets.Where(x => x.UserId == UserId).Select(x => x.BasketId) == null)
        {
            context.Baskets.Add(new Basket
            {
                UserId = UserId,
            });
        }
        Basket_id = context.Baskets.Where(x => x.UserId == UserId).Select(x => x.BasketId).FirstOrDefault();
        user_id = UserId;
    }

    public void LoadProducts()
    {
        using var context = new AfanasyevContext();
        var Products = context.Products.ToList();
        CatalogProducts.ItemsSource = Products;
    }

    public void checkBasket()
    {
        using var context = new AfanasyevContext();
        var Basket = context.Baskets.Where(x=>x.UserId==user_id).Select(x=>x.BasketId);
    }

    public void AddToCart(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var productId = (int)button.Tag;

        using var context = new AfanasyevContext();


        var newId = context.BasketProducts.Any() ? context.BasketProducts.Max(x => x.Id) + 1 : 1;
        context.BasketProducts.Add(new BasketProduct
        {
            Id = newId,
            ProductId = productId,
            BasketId = Basket_id,
            Count = 1
        });
        context.SaveChanges();

        var stackPanel = button.Parent as StackPanel;
        if (stackPanel != null)
        {
            var quantityPanel = stackPanel.Children.OfType<StackPanel>()
                .FirstOrDefault(sp => sp.Orientation == Orientation.Horizontal);

            if (quantityPanel != null)
            {
                button.IsVisible = false;
                quantityPanel.IsVisible = true;
                var countText = quantityPanel.Children.OfType<TextBlock>().FirstOrDefault();
                if (countText != null)
                {
                    countText.Text = "1";
                }
            }
        }
    }

    private void DecreaseQuantity(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var productId = (int)button.Tag;
        using var context = new AfanasyevContext();
        var quantityPanel = button.Parent as StackPanel;
        if (quantityPanel != null)
        {
            var countText = quantityPanel.Children.OfType<TextBlock>().FirstOrDefault();
            if (countText != null && int.TryParse(countText.Text, out int count) && count > 1)
            {
                count--;
                countText.Text = count.ToString();
                var basketProduct = context.BasketProducts
                    .FirstOrDefault(bp => bp.BasketId == Basket_id && bp.ProductId == productId);
                if (basketProduct != null)
                {
                    basketProduct.Count = count;
                    context.SaveChanges();
                }
            }
        }
    }

    private void IncreaseQuantity(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var productId = (int)button.Tag;

        using var context = new AfanasyevContext();

        var quantityPanel = button.Parent as StackPanel;
        if (quantityPanel != null)
        {
            var countText = quantityPanel.Children.OfType<TextBlock>().FirstOrDefault();
            if (countText != null && int.TryParse(countText.Text, out int count))
            {
                count++;
                countText.Text = count.ToString();

                var basketProduct = context.BasketProducts
                    .FirstOrDefault(bp => bp.BasketId == Basket_id && bp.ProductId == productId);
                if (basketProduct != null)
                {
                    basketProduct.Count = count;
                    context.SaveChanges();
                }
            }
        }
    }





    private void BasketOpen_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var BasketWindow = new BasketWindow(user_id);
        BasketWindow.Show();
        this.Close();
    }
}