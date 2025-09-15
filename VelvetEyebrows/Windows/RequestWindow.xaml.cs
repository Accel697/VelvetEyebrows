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
using System.Windows.Shapes;
using VelvetEyebrows.Pages;

namespace VelvetEyebrows.Windows
{
    /// <summary>
    /// Логика взаимодействия для RequestWindow.xaml
    /// </summary>
    public partial class RequestWindow : Window
    {
        public RequestWindow()
        {
            InitializeComponent();
            FrmRequest.Navigate(new RequestList());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrmRequest.GoBack();
        }

        private void FrmRequest_ContentRendered(object sender, EventArgs e)
        {
            if (FrmRequest.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility = Visibility.Hidden;
            }
        }
    }
}
