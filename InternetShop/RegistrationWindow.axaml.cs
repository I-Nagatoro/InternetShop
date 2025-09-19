using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using InternetShop.Models;
using System;
using System.Linq;

namespace InternetShop;

public partial class RegistrationWindow : Window
{
    public RegistrationWindow()
    {
        InitializeComponent();
    }

    public void BackClick(object sender, RoutedEventArgs e)
    {
        MainWindow main = new MainWindow();
        main.Show();
        this.Close();
    }

    public void RegistrationClick(object sender, RoutedEventArgs e)
    {
        using var context = new User025Context();
        var username=UsernameBox.Text;
        var password=PasswordBox.Text;
        var email=EmailBox.Text;
        var phone = PhoneBox.Text;
        var birthday = BirthdayBox.SelectedDate;
        if (username == null || username.Length < 4)
        {
            ErrTxt.Text = "Введите корректное имя пользователя (>=4 символов)";
        }
        else if(context.Users.Where(x => x.Username == username).Any())
        {
            ErrTxt.Text = "Данное имя пользователя уже занято";
        }
        else if (password == null || password.Length < 6)
        {
            ErrTxt.Text = "Введите корректный пароль (>=6 символов)";
        }
        else if (email == null || !IsValidEmail(email))
        {
            ErrTxt.Text = "Введите корректный адрес почты";
        }
        else if (phone == null || phone.Length != 11)
        {
            ErrTxt.Text = "Введите корректный номер телефона";
        } else if (birthday == null)
        {
            ErrTxt.Text = "Введите корректную дату рождения";
        }
        else
        {
            context.Users.Add(new User
            {
                Username = username,
                Email = email,
                Phone = phone,
                Password = password,
                Birthday = DateOnly.FromDateTime(birthday.Value.Date),
                RoleId = 2
            });
            context.SaveChanges();
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }

    public void BackClick(object sender, EventArgs e)
    {
        MainWindow main = new MainWindow();
        main.Show();
        Close();
    }

    public bool IsValidEmail(string email)
    {
        var trimEmail = email.Trim();
        if (trimEmail.EndsWith("."))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimEmail;
        }
        catch
        {
            return false;
        }
    }
}