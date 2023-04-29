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
    /// Interaction logic for IssueCreateView.xaml
    /// </summary>
    public partial class IssueCreateView : UserControl
    {
        public IssueCreateView()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (RecipientText.Text.Length <= 0)
            {
                RecipientText.Visibility = Visibility.Collapsed;
                BloodGroupText1.Visibility = Visibility.Collapsed;
            }

            if (DonationTypeTextBlock.Text.Length <= 0)
            {
                DonationTypeText.Visibility = Visibility.Collapsed;
                BloodGroupText2.Visibility = Visibility.Collapsed;
                DonorText.Visibility = Visibility.Collapsed;
                UnitText.Visibility = Visibility.Collapsed;
                AmountText.Visibility = Visibility.Collapsed;
                FormBorder.Height -= 200;
            }

            if (DoctorText.Text.Length <= 0)
            {
                DoctorText.Visibility = Visibility.Collapsed;
            }
        }
    }
}
