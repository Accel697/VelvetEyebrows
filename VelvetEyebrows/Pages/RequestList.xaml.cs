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
using VelvetEyebrows.Model;

namespace VelvetEyebrows.Pages
{
    /// <summary>
    /// Логика взаимодействия для RequestList.xaml
    /// </summary>
    public partial class RequestList : Page
    {
        DateTime tomorrow = DateTime.Now.AddDays(1);
        private System.Windows.Threading.DispatcherTimer timer;

        public RequestList()
        {
            InitializeComponent();

            var requests = beauty_salonEntities.GetContext().ClientService.Where(r => r.StartTime > DateTime.Now && r.StartTime.Day <= tomorrow.Day).ToList();
            LViewRequests.ItemsSource = requests;

            DataContext = this;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Reload();
        }

        private void btnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddRequest());
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                beauty_salonEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                Reload();
            }
        }

        private void Reload()
        {
            LViewRequests.ItemsSource = null;
            var requests = beauty_salonEntities.GetContext().ClientService.Where(r => r.StartTime > DateTime.Now && r.StartTime.Day <= tomorrow.Day).ToList();
            LViewRequests.ItemsSource = requests;
        }
    }
}
