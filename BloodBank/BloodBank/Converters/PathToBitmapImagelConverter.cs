using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BloodBank.Converters
{
    class PathToBitmapImagelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;

            if (path == null || !File.Exists(path))
                return null;

            //var bmp = new BitmapImage();
            //bmp.BeginInit();
            //bmp.CacheOption = BitmapCacheOption.OnLoad;
            Uri uriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            //bmp.EndInit();
            return uriSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
