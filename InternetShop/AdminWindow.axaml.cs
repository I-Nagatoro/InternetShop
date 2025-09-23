using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace InternetShop;

public partial class AdminWindow : Window
{
    public int user_id;
    public AdminWindow()
    {
        InitializeComponent();
    }

    public AdminWindow(int userId)
    {
        InitializeComponent();
        user_id = userId;
    }

    public void AddProductClick(object sender, RoutedEventArgs e)
    {
        ProductWindow productWindow = new ProductWindow(1, user_id);
        productWindow.Show();
        Close();
    }
    public void UpdateProductClick(object sender, RoutedEventArgs e)
    {
        ProductWindow productWindow = new ProductWindow(0,user_id);
        productWindow.Show();
        Close();
    }
    public void DeleteProductClick(object sender, RoutedEventArgs e)
    {
        DeleteProductWindow deleteProductWindow = new DeleteProductWindow(user_id);
        deleteProductWindow.Show();
        Close();
    }
    public void ShowUpdateOrdersClick(object sender, RoutedEventArgs e)
    {
        EditOrderWindow editOrderWindow = new EditOrderWindow(user_id);
        editOrderWindow.Show();
        Close();
    }

    public void ExitClick(object sender, RoutedEventArgs e)
    {
        CatalogWindow catalogWindow = new CatalogWindow(user_id);
        catalogWindow.Show();
        Close();
    }
}