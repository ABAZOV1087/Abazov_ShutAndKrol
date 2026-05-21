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
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            dgComplaints.ItemsSource = Core.Context.Complaints.ToList();
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            var selectedComplaint = dgComplaints.SelectedItem as Complaints;
            if (selectedComplaint == null)
            {
                MessageBox.Show("Выберите жалобу из списка.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Core.Context.Complaints.Remove(selectedComplaint);
            Core.Context.SaveChanges();

            UpdateGrid();
            MessageBox.Show("Жалоба отклонена и удалена из списка.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnFreezeBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedComplaint = dgComplaints.SelectedItem as Complaints;
            if (selectedComplaint == null)
            {
                MessageBox.Show("Выберите жалобу.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedComplaint.TargetBookID == null)
            {
                MessageBox.Show("Эта жалоба не связана с книгой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int bookId = selectedComplaint.TargetBookID.Value;
            var book = Core.Context.Books.FirstOrDefault(b => b.ID == bookId);

            if (book != null)
            {
                book.IsFrozen = true;
                Core.Context.Complaints.Remove(selectedComplaint);
                Core.Context.SaveChanges();

                UpdateGrid();
                MessageBox.Show("Книга успешно заморожена. Жалоба закрыта.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnFreezeUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedComplaint = dgComplaints.SelectedItem as Complaints;
            if (selectedComplaint == null)
            {
                MessageBox.Show("Выберите жалобу.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Users userToFreeze = null;

            if (selectedComplaint.TargetReviewID != null)
            {
                int reviewId = selectedComplaint.TargetReviewID.Value;
                var review = Core.Context.Reviews.FirstOrDefault(r => r.ID == reviewId);
                if (review != null)
                {
                    userToFreeze = Core.Context.Users.FirstOrDefault(u => u.ID == review.UserID);
                }
            }
            else if (selectedComplaint.TargetBookID != null)
            {
                int bookId = selectedComplaint.TargetBookID.Value;
                var book = Core.Context.Books.FirstOrDefault(b => b.ID == bookId);
                if (book != null)
                {
                    userToFreeze = Core.Context.Users.FirstOrDefault(u => u.ID == book.AuthorID);
                }
            }

            if (userToFreeze != null)
            {
                userToFreeze.IsFrozen = true;
                Core.Context.Complaints.Remove(selectedComplaint);
                Core.Context.SaveChanges();

                UpdateGrid();
                MessageBox.Show($"Пользователь {userToFreeze.Login} успешно заморожен. Жалоба закрыта.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не удалось определить нарушителя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
