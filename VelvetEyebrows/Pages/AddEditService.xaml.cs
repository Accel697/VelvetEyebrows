using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using System.IO;
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
                btnDelete.Visibility = Visibility.Visible;
            }

            DataContext = _service;
        }

        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        {
            string imagesPath = "C:\\NATK\\developmenttools\\VelvetEyebrows\\VelvetEyebrows\\Resources\\Услуги салона красоты";
            OpenFileDialog getImageDialog = new OpenFileDialog();

            getImageDialog.Filter = "Файды изображений: (*.png, *.jpg, *.jpeg)| *.png; *.jpg; *.jpeg";
            getImageDialog.InitialDirectory = $"{imagesPath}";
            if (getImageDialog.ShowDialog() == true)
            {
                if (!getImageDialog.FileName.StartsWith(imagesPath))
                {
                    string targetPath = $"{imagesPath}\\{getImageDialog.SafeFileName}";
                    File.Copy(getImageDialog.FileName, targetPath, true);
                }

                _service.MainImagePath = $"Услуги салона красоты\\{getImageDialog.SafeFileName}";
                img.Source = new BitmapImage(new Uri(getImageDialog.FileName));
            }
        }

        private void btnClearImage_Click(object sender, RoutedEventArgs e)
        {
            _service.MainImagePath = null;
            img.Source = null;
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
                    serviceInDb.Discount = _service.Discount / 100;
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var context = beauty_salonEntities.GetContext();

            if (MessageBox.Show($"Вы действительно хотите удалить: {_service.Title}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    context.Service.Remove(_service);
                    context.SaveChanges();
                    MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
