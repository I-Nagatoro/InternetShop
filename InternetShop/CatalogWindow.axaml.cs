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
using System.Xml.Serialization;

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

        using var context = new User025Context();
        var basket = context.Baskets.FirstOrDefault(x => x.UserId == UserId);

        if (basket == null)
        {
            basket = new Basket { UserId = UserId };
            context.Baskets.Add(basket);
            context.SaveChanges();
        }

        Basket_id = basket.BasketId;
        LoadProducts();
        if (CheckAdm(user_id))
        {
            AdmBtn.IsVisible=true;
        }
    }

    public void AdminClick(object sender, RoutedEventArgs e)
    {
        AdminWindow adm = new AdminWindow(user_id);
        adm.Show();
        Close();
    }

    public bool CheckAdm(int user_id)
    {
        using var context = new User025Context();
        if (context.Users.Where(x=>x.UserId == user_id).Select(x => x.RoleId).FirstOrDefault() == 1)
        {
            return true;
        }
        return false;
    }

    public void LoadProducts()
    {
        using var context = new User025Context();
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

        using var context = new User025Context();

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

        using var context = new User025Context();
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

        using var context = new User025Context();
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

    public void ExitClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void SortAlphComboBox(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void SortCostComboBox(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }

    public void ApplyFilters()
    {
        IEnumerable<ProductItem> rawList = _productItems;
        if (SearchBox.Text != null)
        {
            var search = SearchBox.Text.ToLower();
            rawList = rawList.Where(x => x.Product.ProductName.ToLower().Contains(search)).ToList();
        }

        switch (comboSortName.SelectedIndex)
        {
            case 0:
                rawList = rawList.OrderBy(x => x.Product.ProductName);
                break;
            case 1:
                rawList = rawList.OrderByDescending(x => x.Product.ProductName);
                break;
        }

        switch (comboSortCost.SelectedIndex)
        {
            case 0:
                rawList = rawList.OrderBy(x => x.Product.Cost);
                break;
            case 1:
                rawList = rawList.OrderByDescending(x => x.Product.Cost);
                break;
        }
        CatalogProducts.ItemsSource = null;
        CatalogProducts.ItemsSource = rawList;
    }

    private void SearchBoxKeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        ApplyFilters();
    }

    public void DropSortClick(object sender, RoutedEventArgs e)
    {
        SearchBox.Text = "";
        comboSortCost.SelectedIndex = -1;
        comboSortName.SelectedIndex = -1;
        CatalogProducts.ItemsSource = null;
        CatalogProducts.ItemsSource = _productItems;
    }
}