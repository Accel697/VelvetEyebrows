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
using VelvetEyebrows.Windows;

namespace VelvetEyebrows.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private bool adminMode = false;

        public string[] SortingList { get; set; } =
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию"
        };

        public string[] FilterList { get; set; } =
        {
            "Все диапазоны",
            "0% - 5%",
            "5% - 15%",
            "15% - 30%",
            "30% - 70%",
            "70% - 100%"
        };

        public MainPage()
        {
            InitializeComponent();

            var services = beauty_salonEntities.GetContext().Service.ToList();
            LViewServices.ItemsSource = services;

            DataContext = this;

            txtAllAmount.Text = services.Count.ToString();

            UpdateData();
        }

        private void UpdateData()
        {
            var result = beauty_salonEntities.GetContext().Service.ToList();

            if (cbSorting.SelectedIndex == 1)
            {
                result = result.OrderBy(s => s.Cost).ToList();
            }
            if (cbSorting.SelectedIndex == 2)
            {
                result = result.OrderByDescending(a => a.Cost).ToList();
            }

            if (cbFilter.SelectedIndex == 1)
            {
                result = result.Where(s => s.Discount >= 0 && s.Discount < 5).ToList();
            }
            if (cbFilter.SelectedIndex == 2)
            {
                result = result.Where(s => s.Discount >= 5 && s.Discount < 15).ToList();
            }
            if (cbFilter.SelectedIndex == 3)
            {
                result = result.Where(s => s.Discount >= 15 && s.Discount < 30).ToList();
            }
            if (cbFilter.SelectedIndex == 4)
            {
                result = result.Where(s => s.Discount >= 30 && s.Discount < 70).ToList();
            }
            if (cbFilter.SelectedIndex == 5)
            {
                result = result.Where(s => s.Discount >= 70 && s.Discount < 100).ToList();
            }

            result = result.Where(p => p.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();

            txtResultAmount.Text = result.Count.ToString();

            LViewServices.ItemsSource = result;
        }

        private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void tbAdminCode_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (tbAdminCode.Text == "0000" && adminMode == false)
            {
                adminMode = true;
                tbAdminCode.Clear();
                MessageBox.Show("Режим администратора активирован", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                btnToRequests.Visibility = Visibility.Visible;
                btnAddService.Visibility = Visibility.Visible;
            }
        }

        private void btnToRequests_Click(object sender, RoutedEventArgs e)
        {
            RequestWindow requestWindow = new RequestWindow();
            requestWindow.Show();
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditService(null));
        }

        private void LViewServices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (adminMode == true)
            {
                NavigationService.Navigate(new AddEditService(LViewServices.SelectedItem as Service));
            }
        }

        private void btnEditService_Click(object sender, RoutedEventArgs e)
        {
            if (adminMode == true)
            {
                Button button = sender as Button;

                var item = button.DataContext;

                if (item is Service service)
                {
                    NavigationService.Navigate(new AddEditService(service));
                }
            }
        }

        private void btnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (adminMode == true)
            {
                Button button = sender as Button;

                var item = button.DataContext;

                if (item is Service service)
                {
                    var context = beauty_salonEntities.GetContext();

                    if (MessageBox.Show($"Вы действительно хотите удалить: {service.Title}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            context.Service.Remove(service);
                            context.SaveChanges();
                            MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            Reload();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Reload();
            }
        }

        private void Reload()
        {
            beauty_salonEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            var services = beauty_salonEntities.GetContext().Service.ToList();
            LViewServices.ItemsSource = null;
            LViewServices.ItemsSource = services;
            txtAllAmount.Text = services.Count.ToString();
            UpdateData();
        }
    }
}
