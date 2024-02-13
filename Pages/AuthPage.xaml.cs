using Gubaidullin41size.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var user = Gubaidullin41Entities.GetContext().User.ToList().Find(u => u.UserLogin == login && u.UserPassword == password);
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
    }
}
