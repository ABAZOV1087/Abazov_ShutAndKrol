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
    public partial class ListsPage : Page
    {
        public ListsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var filterStatuses = Core.Context.ReadingListStatuses.ToList();
            filterStatuses.Insert(0, new ReadingListStatuses { ID = 0, Name = "Все списки" });
            cbListFilter.ItemsSource = filterStatuses;
            cbListFilter.DisplayMemberPath = "Name";
            cbListFilter.SelectedIndex = 0;

            UpdateUserList();
        }

        private void UpdateUserList()
        {
            if (Core.CurrentUser == null) return;

            var userList = Core.Context.ReadingLists.Where(rl => rl.UserID == Core.CurrentUser.ID).ToList();

            if (cbListFilter.SelectedIndex > 0)
            {
                int selectedStatusId = Convert.ToInt32(cbListFilter.SelectedValue);
                userList = userList.Where(rl => rl.StatusID == selectedStatusId).ToList();
            }

            lvUserBooks.ItemsSource = userList;
        }

        private void cbListFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUserList();
        }

        private void lvUserBooks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedRecord = lvUserBooks.SelectedItem as ReadingLists;
            if (selectedRecord != null && selectedRecord.Books != null)
            {
                NavigationService.Navigate(new BookDetailsPage(selectedRecord.Books));
            }
        }
    }
}
