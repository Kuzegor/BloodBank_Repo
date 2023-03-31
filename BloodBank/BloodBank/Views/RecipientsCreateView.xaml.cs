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

namespace BloodBank.Views
{
    /// <summary>
    /// Interaction logic for RecipientsCreateView.xaml
    /// </summary>
    public partial class RecipientsCreateView : UserControl
    {
        public RecipientsCreateView()
        {
            InitializeComponent();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            UploadButton.Visibility = Visibility.Collapsed;
            DeleteButton.Visibility = Visibility.Visible;
            UploadButton.IsEnabled = false;
            DeleteButton.IsEnabled = true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            UploadButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Collapsed;
            UploadButton.IsEnabled = true;
            DeleteButton.IsEnabled = false;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (ImagePicture.Source == null)
            {
                UploadButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Collapsed;
                UploadButton.IsEnabled = true;
                DeleteButton.IsEnabled = false;
            }
            else
            {
                UploadButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Visible;
                UploadButton.IsEnabled = false;
                DeleteButton.IsEnabled = true;
            }
        }
    }
}
