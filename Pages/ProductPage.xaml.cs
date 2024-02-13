using Gubaidullin41size.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private int FullCount;
        public ProductPage(User user)
        {
            InitializeComponent();

            if (user == null)
            {
                UserNameTextBlock.Text = "Вы авторизованы как Гость";
            }
            else
            {
                UserNameTextBlock.Text = $"Вы авторизованы как {user.UserName} {user.UserSurname} {user.UserPatronymic}";

                switch (user.UserRole)
                {
                    case 1: RoleTextBlock.Text = "Роль: Клиент"; break;
                    case 2: RoleTextBlock.Text = "Роль: Менеджер"; break;
                    case 3: RoleTextBlock.Text = "Роль: Администратор"; break;
                    default:
                        break;
                }
            }

            

            ProductListView.ItemsSource = Gubaidullin41Entities.GetContext().Product.ToList();
            ComboType.SelectedIndex = 0;
            FullCount = Gubaidullin41Entities.GetContext().Product.Count();
            UpdateProducts();
        }

        public void UpdateProducts()
        {
            var count = 0;
            var currentProducts = Gubaidullin41Entities.GetContext().Product.ToList();

            if (ComboType.SelectedIndex == 1)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 9.99)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount > 9.99 && p.ProductDiscountAmount <= 14.99)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentProducts = currentProducts.Where(p => (p.ProductDiscountAmount > 14.99 && p.ProductDiscountAmount <= 100)).ToList();
            }

            currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(SearchTBox.Text.ToLower())).ToList();

            if (UpRadioBtn.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
            }
            if (DownRadioBtn.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }
            count = currentProducts.Count;
            CountTextBlock.Text = $"Выведено {count} из {FullCount}";
            ProductListView.ItemsSource = currentProducts;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void SearchTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();

        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();

        }

        private void UpRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();

        }

        private void DownRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();

        }
    }
}
