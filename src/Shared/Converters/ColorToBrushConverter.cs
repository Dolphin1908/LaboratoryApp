using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LaboratoryApp.src.Shared.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString)
            {
                colorString = colorString.ToLower();
                try
                {
                    //if (colorstring == "ffffff" || colorstring == "")
                    //    colorstring = "bdbdbd";
                    if (colorString.StartsWith("#"))
                        colorString = colorString.Substring(1);
                    var color = (Color)ColorConverter.ConvertFromString("#" + colorString);
                    return new SolidColorBrush(color);
                }
                catch
                {
                    var color = (Color)ColorConverter.ConvertFromString("#bdbdbd");
                    return new SolidColorBrush(color); // Default background
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
