using Avalonia.Controls;
using Avalonia.Interactivity;
using InternetShop.Models;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Transactions;
using Tmds.DBus.Protocol;

namespace InternetShop
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();


        }

        public void EnterButton(object sender, RoutedEventArgs e)
        {
            var username = usernameBox.Text;
            var password = passwordBox.Text;
            using var context = new AfanasyevContext();
                var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    var catalogWindow = new CatalogWindow(user.Username, user.UserId);
                    catalogWindow.Show();
                    Close();
                }
                else
                {
                    var error = "Неверное имя пользователя или пароль";
                    var errorWindow = new ErrorWindow(error);
                    errorWindow.Show();
                }
        }
    }
}