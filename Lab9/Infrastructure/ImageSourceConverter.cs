using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows;

namespace Lab9.Infrastructure
{
    public class ImageSourceConverter : IValueConverter
    {
        string imageDirectory = Directory.GetCurrentDirectory();

        string ImageDirectory
        {
            get
            {
                return Path.Combine(imageDirectory, "images");
            }
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return Path.Combine(ImageDirectory, (String)value);
        }

        public Object ConvertBack(Object value, Type targetType, Object Parameterm, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
