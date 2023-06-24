using BloodBank.ViewModels;
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
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindowViewModel.Instance.CurrentUserIsAdmin)
            {
                DoctorsRadio.Visibility = Visibility.Visible;
                RolesRadio.Visibility = Visibility.Visible;                
            }
            else
            {
                DoctorsRadio.Visibility = Visibility.Collapsed;
                RolesRadio.Visibility = Visibility.Collapsed;
            }
        }
    }
}
