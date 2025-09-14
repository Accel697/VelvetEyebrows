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
using Microsoft.Win32;
using VelvetEyebrows.Model;
using VelvetEyebrows.Services;

namespace VelvetEyebrows.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditService.xaml
    /// </summary>
    public partial class AddEditService : Page
    {
        Service _service = new Service();

        public AddEditService(Service service)
        {
            InitializeComponent();

            if (service != null)
            {
                _service = service;
            }

            DataContext = _service;
        }

        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog getImageDialog = new OpenFileDialog();

            getImageDialog.Filter = "Файды изображений: (*.png, *.jpg, *.jpeg)| *.png; *.jpg; *.jpeg";
            getImageDialog.InitialDirectory = "C:\\NATK\\developmenttools\\VelvetEyebrows\\VelvetEyebrows\\Resources\\Услуги салона красоты";
            if (getImageDialog.ShowDialog() == true)
            {
                _service.MainImagePath = $"Услуги салона красоты\\{getImageDialog.SafeFileName}";
                img.Source = new BitmapImage(new Uri(getImageDialog.FileName));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var context = beauty_salonEntities.GetContext();

            DataValidator validator = new DataValidator();
            var (isValid, errors) = validator.ServiceValidator(_service);

            if (!isValid )
            {
                MessageBox.Show(string.Join("\n", errors), "Ошибки валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_service.ID == 0)
            {
                context.Service.Add(_service);
                NavigationService.GoBack();
            }
            else
            {
                var serviceInDb = context.Service.FirstOrDefault(s => s.ID == _service.ID);
                if (serviceInDb != null)
                {
                    serviceInDb.Title = _service.Title;
                    serviceInDb.Cost = _service.Cost;
                    serviceInDb.DurationInSeconds = _service.DurationInSeconds;
                    serviceInDb.Description = _service.Description;
                    serviceInDb.Discount = _service.Discount;
                    serviceInDb.MainImagePath = _service.MainImagePath;
                }
                else
                {
                    MessageBox.Show("Услуга не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
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
