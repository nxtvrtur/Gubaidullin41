using Gubaidullin41size.Helpers;
using Gubaidullin41size.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Gubaidullin41size.Pages
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private int FullCount;
        User currentUser;
        int newOrderId;
        public static bool btnActivity = false;

        List<OrderProduct> selectedOrderProducts = new List<OrderProduct>();
        List<Product> selectedProducts = new List<Product>();

        public ProductPage(User user)
        {
            InitializeComponent();
            currentUser = user;
            if (!btnActivity)
            {
                OrderBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                OrderBtn.Visibility = Visibility.Visible;
            }

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

            

            ProductListView.ItemsSource = Gubaidullin41Entities1.GetContext().Product.ToList();
            ComboType.SelectedIndex = 0;
            FullCount = Gubaidullin41Entities1.GetContext().Product.Count();
            UpdateProducts();
        }

        public void UpdateProducts()
        {
            var count = 0;
            var currentProducts = Gubaidullin41Entities1.GetContext().Product.ToList();

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

        private void AddToOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListView.SelectedIndex >= 0)
            {
                var prod = ProductListView.SelectedItem as Product;

                //int newOrderID = selectedOrderProducts.Last().Order.OrderID;
                var newOrderProd = new OrderProduct();
                newOrderProd.OrderID = newOrderId;

                newOrderProd.ProductArticleNumber = prod.ProductArticleNumber;
                newOrderProd.Amount = 1;
                var selOP = selectedOrderProducts.Where(p => Equals(p.ProductArticleNumber, prod.ProductArticleNumber));

                if (selOP.Count() == 0)
                {
                    selectedOrderProducts.Add(newOrderProd);
                    selectedProducts.Add(prod);
                }
                else
                {
                    foreach (OrderProduct p in selectedOrderProducts)
                    {
                        if (p.ProductArticleNumber == prod.ProductArticleNumber)
                            p.Amount++;
                    }
                }

                OrderBtn.Visibility = Visibility.Visible;
                ProductListView.SelectedIndex = -1;

                UpdateProducts();

            }
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow(selectedOrderProducts, selectedProducts, currentUser).ShowDialog();
            btnActivity = true;
            UpdateProducts();
        }
    }
}
