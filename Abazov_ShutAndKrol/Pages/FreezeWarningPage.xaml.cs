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
    public partial class FreezeWarningPage : Page
    {
        public FreezeWarningPage()
        {
            InitializeComponent();
        }

        private void btnUnfreezeRequest_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUser == null) return;

            string appealMessage = Microsoft.VisualBasic.Interaction.InputBox("Опишите подробно причину, по которой администратор должен разблокировать ваш профиль:", "Апелляция на разморозку");
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
            MessageBox.Show("Ваша апелляция успешно отправлена администрации платформы.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
