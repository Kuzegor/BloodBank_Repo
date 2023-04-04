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
    /// Interaction logic for DoctorsView.xaml
    /// </summary>
    public partial class DoctorsView : UserControl
    {
        public DoctorsView()
        {
            InitializeComponent();
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            ListViewItem listViewItem = e.Source as ListViewItem;
            ButtonsMenuPopup.PlacementTarget = listViewItem;
            ButtonsMenuPopup.IsOpen = true;
        }

        private void ListViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ButtonsMenuPopup.IsMouseOver == false)
            {
                ButtonsMenuPopup.IsOpen = false;
            }
        }

        private void Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonsMenuPopup.IsOpen = false;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SetVisibility();
        }

        private void Grid_LayoutUpdated(object sender, EventArgs e)
        {
            SetVisibility();
        }

        private void SetVisibility()
        {
            if (SelectButton.Command != null)
            {
                SelectButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
                CreateButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                SelectButton.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                CreateButton.Visibility = Visibility.Visible;
            }
        }
    }
}
