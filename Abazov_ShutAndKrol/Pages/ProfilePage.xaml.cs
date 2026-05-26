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
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null) return;

            tblLogin.Text = Core.CurrentUser.Login;
            tblDisplayName.Text = Core.CurrentUser.DisplayName;
            tblEmail.Text = Core.CurrentUser.Email;

            var role = Core.Context.Roles.FirstOrDefault(r => r.ID == Core.CurrentUser.RoleID);
            if (role != null)
            {
                tblRole.Text = role.Name;
            }

            int listsCount = Core.Context.ReadingLists.Count(rl => rl.UserID == Core.CurrentUser.ID);
            tblBooksInListsCount.Text = listsCount.ToString();

            if (Core.CurrentUser.RoleID == 2)
            {
                borderAuthorStats.Visibility = Visibility.Visible;
                int authorBooksCount = Core.Context.Books.Count(b => b.AuthorID == Core.CurrentUser.ID);
                tblAuthorBooksCount.Text = authorBooksCount.ToString();
            }
            if (Core.CurrentUser == null) return;

            if (Core.CurrentUser.RoleID != 1)
            {
                btnRequestAuthor.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;

            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.SidebarBorder.Visibility = Visibility.Collapsed;
                mainWindow.btnAdmin.Visibility = Visibility.Collapsed;
                mainWindow.btnAuthor.Visibility = Visibility.Collapsed;
                mainWindow.btnFreezeWarning.Visibility = Visibility.Collapsed;
                mainWindow.MainFrame.Navigate(new LoginPage());
            }
        }
        private void btnRequestAuthor_Click(object sender, RoutedEventArgs e)
        {
            string portfolio = Microsoft.VisualBasic.Interaction.InputBox("Укажите ссылки на ваши публикации или кратко опишите ваше творчество:", "Заявка на статус автора");
            if (string.IsNullOrWhiteSpace(portfolio)) return;

            Complaints authorRequest = new Complaints
            {
                SenderID = Core.CurrentUser.ID,
                TargetBookID = null,
                TargetReviewID = null,
                Reason = $"[ЗАЯВКА В АВТОРЫ] {portfolio}",
                CreatedAt = DateTime.Now
            };

            Core.Context.Complaints.Add(authorRequest);
            Core.Context.SaveChanges();
            MessageBox.Show("Ваша заявка успешно отправлена на рассмотрение администрации!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnUnfreezeRequest_Click(object sender, RoutedEventArgs e)
        {
            string appealMessage = Microsoft.VisualBasic.Interaction.InputBox("Опишите подробно причину, по которой администратор должен разблокировать ваш профиль/контент:", "Апелляция на разморозку");
            if (string.IsNullOrWhiteSpace(appealMessage)) return;

            Complaints unfreezeAppeal = new Complaints
            {
                SenderID = Core.CurrentUser.ID,
                TargetBookID = null,
                TargetReviewID = null,
                Reason = $"[АПЕЛЛЯЦИЯ НА РАЗМОРОЗКУ] {appealMessage}",
                CreatedAt = DateTime.Now
            };

            Core.Context.Complaints.Add(unfreezeAppeal);
            Core.Context.SaveChanges();
            MessageBox.Show("Ваша заявка на разморозку успешно отправлена и находится на рассмотрении администрации.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
