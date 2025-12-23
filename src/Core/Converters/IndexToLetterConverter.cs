using System.Globalization;
using System.Windows.Data;

namespace LaboratoryApp.src.Core.Converters
{
    public class IndexToLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Chúng ta sẽ lấy Index từ ItemsControl
            // (Code XAML sẽ truyền ItemsControl.AlternationIndex vào đây)
            if (value is int index)
            {
                // ASCII: 65 là 'A'
                return ((char)('A' + index)).ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
