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
    /// Interaction logic for BloodCollectionCreateView.xaml
    /// </summary>
    public partial class BloodCollectionCreateView : UserControl
    {
        public BloodCollectionCreateView()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (DonorText.Text.Length <= 0)
            {
                DonorText.Visibility = Visibility.Collapsed;
                BloodGroupText.Visibility = Visibility.Collapsed; 
            }

            if (DoctorText.Text.Length <= 0)
            {
                DoctorText.Visibility = Visibility.Collapsed;
            }
        }
    }
}
