using System.Windows;

namespace UniversityWpf;

public partial class LoginWindow : Window
{
    private const string ValidLogin = "admin";
    private const string ValidPassword = "admin";

    public LoginWindow()
    {
        InitializeComponent();
        LoginBox.Focus();
    }

    private void LoginClick(object sender, RoutedEventArgs e)
    {
        var login = LoginBox.Text.Trim();
        var password = PasswordBox.Password;

        if (login == ValidLogin && password == ValidPassword)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }
        else
        {
            ErrorText.Text = "Неверный логин или пароль";
            ErrorText.Visibility = Visibility.Visible;
            PasswordBox.Clear();
        }
    }
}
