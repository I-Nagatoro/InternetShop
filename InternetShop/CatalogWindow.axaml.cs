using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace InternetShop;

public partial class CatalogWindow : Window
{
    public int Basket_id;
    public int user_id;
    private ObservableCollection<ProductItem> _productItems;

    public class ProductItem : INotifyPropertyChanged
    {
        private int _quantity;

        public Product Product { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsInBasket));
                }
            }
        }

        public bool IsInBasket => Quantity > 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public CatalogWindow()
    {
        InitializeComponent();
    }

    public CatalogWindow(int UserId)
    {
        InitializeComponent();
        user_id = UserId;

        using var context = new AfanasyevContext();
        var basket = context.Baskets.FirstOrDefault(x => x.UserId == UserId);

        if (basket == null)
        {
            basket = new Basket { UserId = UserId };
            context.Baskets.Add(basket);
            context.SaveChanges();
        }

        Basket_id = basket.BasketId;
        LoadProducts();
    }

    public void LoadProducts()
    {
        using var context = new AfanasyevContext();
        var products = context.Products.ToList();
        var basketProducts = context.BasketProducts
            .Where(bp => bp.BasketId == Basket_id)
            .ToDictionary(bp => bp.ProductId, bp => bp.Count);

        _productItems = new ObservableCollection<ProductItem>(
            products.Select(p => new ProductItem
            {
                Product = p,
                Quantity = basketProducts.ContainsKey(p.ProductId) ? basketProducts[p.ProductId] : 0
            })
        );

        CatalogProducts.ItemsSource = _productItems;
    }

    public void AddToCart(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var productId = (int)button.Tag;

        using var context = new AfanasyevContext();

        var basketProduct = context.BasketProducts
            .FirstOrDefault(bp => bp.BasketId == Basket_id && bp.ProductId == productId);

        if (basketProduct == null)
        {
            var newId = context.BasketProducts.Any() ? context.BasketProducts.Max(x => x.Id) + 1 : 1;
            basketProduct = new BasketProduct
            {
                Id = newId,
                ProductId = productId,
                BasketId = Basket_id,
                Count = 1
            };
            context.BasketProducts.Add(basketProduct);
        }
        else
        {
            basketProduct.Count++;
        }
        context.SaveChanges();
        var productItem = _productItems.FirstOrDefault(pi => pi.Product.ProductId == productId);
        if (productItem != null)
        {
            productItem.Quantity = basketProduct.Count;
        }
    }

    private void DecreaseQuantity(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var productId = (int)button.Tag;

        using var context = new AfanasyevContext();
        var basketProduct = context.BasketProducts
            .FirstOrDefault(bp => bp.BasketId == Basket_id && bp.ProductId == productId);

        if (basketProduct != null)
        {
            if (basketProduct.Count > 1)
            {
                basketProduct.Count--;
                context.SaveChanges();

                var productItem = _productItems.FirstOrDefault(pi => pi.Product.ProductId == productId);
                if (productItem != null)
                {
                    productItem.Quantity = basketProduct.Count;
                }
            }
            else
            {
                context.BasketProducts.Remove(basketProduct);
                context.SaveChanges();

                var productItem = _productItems.FirstOrDefault(pi => pi.Product.ProductId == productId);
                if (productItem != null)
                {
                    productItem.Quantity = 0;
                }
            }
        }
    }

    private void IncreaseQuantity(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var productId = (int)button.Tag;

        using var context = new AfanasyevContext();
        var basketProduct = context.BasketProducts
            .FirstOrDefault(bp => bp.BasketId == Basket_id && bp.ProductId == productId);

        if (basketProduct != null)
        {
            basketProduct.Count++;
            context.SaveChanges();

            var productItem = _productItems.FirstOrDefault(pi => pi.Product.ProductId == productId);
            if (productItem != null)
            {
                productItem.Quantity = basketProduct.Count;
            }
        }
    }

    private void BasketOpen_Click(object? sender, RoutedEventArgs e)
    {
        var BasketWindow = new BasketWindow(user_id);
        BasketWindow.Show();
        this.Close();
    }
}