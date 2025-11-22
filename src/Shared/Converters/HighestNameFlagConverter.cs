using LaboratoryApp.Domain.Helpers;
using System.Globalization;
using System.Windows.Data;

namespace LaboratoryApp.src.Shared.Converters
{
    public class HighestNameFlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum roleEnum)
            {
                // Gọi hàm extension bạn vừa thêm
                return roleEnum.GetHighestDisplayName();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
