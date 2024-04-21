using Gubaidullin41size.Helpers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Gubaidullin41size.Pages
{
    /// <summary>
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private async void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBlock.Text;
            var password = PasswordTextBlock.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) 
            {
                MessageBox.Show("Есть пустые поля!");
                return;
            }
            var user = Gubaidullin41Entities1.GetContext().User.ToList().Find(u => u.UserLogin == login && u.UserPassword == password);
            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                LoginTextBlock.Clear();
                PasswordTextBlock.Clear();
            }
            else
            {
                MessageBox.Show("Введены неправильные данные");
                SignInBtn.IsEnabled = false;
                await Task.Delay(TimeSpan.FromSeconds(10));
                SignInBtn.IsEnabled = true;
            }
        }

        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
                Manager.MainFrame.Navigate(new ProductPage(null));
                LoginTextBlock.Clear();
                PasswordTextBlock.Clear();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBlock.Text;
            var password = PasswordTextBlock.Password;
            if (CheckPassword(password))
            {
                MessageBox.Show("Успешно");
                return;
            }
        }
        private bool CheckPassword(string password)
        {
            var builder = new StringBuilder();
            var charCount = 0;
            var lowLetter = 0;
            var bigLetter = 0;
            var digitCount = 0;
            if (password.Length < 8)
            {
                builder.AppendLine("Пароль должен содержать минимум 8 символов");
            }
            else
            {
                foreach (var l in password)
                {
                    if (char.IsDigit(l))
                    {
                        digitCount++;
                    }
                    if (char.IsLower(l))
                    {
                        lowLetter++;
                    }
                    if (char.IsUpper(l))
                    {
                        bigLetter++;
                    }
                    if (char.IsSymbol(l))
                    {
                        charCount++;
                    }
                }
                if (digitCount == 0)
                {
                    builder.AppendLine("Пароль должен содержать цифру");
                }
                if (lowLetter == 0)
                {
                    builder.AppendLine("Пароль должен содержать маленькую букву");
                }
                if (bigLetter == 0)
                {
                    builder.AppendLine("Пароль должен содержать большую букву");
                }
                if (charCount == 0)
                {
                    builder.AppendLine("Пароль должен содержать символ");
                }
            }

            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString());
                return false;
            }

            return true;
        }

    }
}
