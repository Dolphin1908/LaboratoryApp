﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LaboratoryApp.Support.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString)
            {
                try
                {
                    if (colorString == "FFFFFF" || colorString == "")
                        colorString = "BDBDBD";
                    var color = (Color)ColorConverter.ConvertFromString("#" + colorString);
                    return new SolidColorBrush(color);
                }
                catch
                {
                    var color = (Color)ColorConverter.ConvertFromString("#BDBDBD");
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
