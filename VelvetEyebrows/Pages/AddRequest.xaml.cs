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
using VelvetEyebrows.Services;

namespace VelvetEyebrows.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddRequest.xaml
    /// </summary>
    public partial class AddRequest : Page
    {
        DateTime endTime;

        ClientService _request = new ClientService();

        public AddRequest()
        {
            InitializeComponent();

            cbClient.ItemsSource = beauty_salonEntities.GetContext().Client.ToList();
            cbService.ItemsSource = beauty_salonEntities.GetContext().Service.ToList();

            DataContext = _request;
            _request.DateStart = DateTime.Now;
        }

        private void CalculateEndTime()
        {
            if (_request.ServiceID != 0 && tbStartTime.Text.Length == 5)
            {
                var service = beauty_salonEntities.GetContext().Service.FirstOrDefault(s => s.ID == _request.ServiceID);
                endTime = _request.StartTime.AddSeconds(service.DurationInSeconds);
                tblEndTime.Text = $"{endTime: HH:mm}";
            }
        }

        private void cbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateEndTime();
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _request.DateStart = DateTime.Parse(dpDate.SelectedDate.ToString());
        }

        private void tbTime_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CalculateEndTime();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var context = beauty_salonEntities.GetContext();

            _request.StartTime = _request.DateStart.Date + _request.StartTime.TimeOfDay;

            DataValidator validator = new DataValidator();
            var (isValid, errors) = validator.RequestValidator(_request);

            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", errors), "Ошибки валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            context.ClientService.Add(_request);
            NavigationService.GoBack();
            try
            {
                context.SaveChanges();
                MessageBox.Show("Данные сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
