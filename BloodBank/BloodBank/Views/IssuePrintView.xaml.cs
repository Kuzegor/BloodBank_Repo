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
    /// Interaction logic for IssuePrintView.xaml
    /// </summary>
    public partial class IssuePrintView : UserControl
    {
        public IssuePrintView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                this.PrintButton.Visibility = Visibility.Hidden;
                this.GoBackButton.Visibility = Visibility.Hidden;
                this.PrintScroll.Width = 808.7;
                this.PrintScroll.Height = 1137.5;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(this.PrintBorder, "Print UserControl");
                }
            }
            finally
            {
                this.IsEnabled = true;
                this.PrintButton.Visibility = Visibility.Visible;
                this.GoBackButton.Visibility = Visibility.Visible;
                this.PrintScroll.Width = Double.NaN;
                this.PrintScroll.Height = Double.NaN;
            }
        }
    }
}
