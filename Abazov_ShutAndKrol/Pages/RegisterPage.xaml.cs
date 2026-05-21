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

namespace Abazov_ShutAndKrol.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbLogin.Text) || string.IsNullOrWhiteSpace(tbPassword.Text) || string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля (*).", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var userExists = Core.Context.Users.Any(u => u.Login == tbLogin.Text || u.Email == tbEmail.Text);
            if (userExists)
            {
                MessageBox.Show("Пользователь с таким логином или Email уже зарегистрирован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Users newUser = new Users
            {
                Login = tbLogin.Text,
                Password = tbPassword.Text,
                Email = tbEmail.Text,
                DisplayName = string.IsNullOrWhiteSpace(tbDisplayName.Text) ? tbLogin.Text : tbDisplayName.Text,
                RoleID = 1,
                IsFrozen = false
            };

            Core.Context.Users.Add(newUser);
            Core.Context.SaveChanges();

            MessageBox.Show("Вы успешно зарегистрировались!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new LoginPage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
