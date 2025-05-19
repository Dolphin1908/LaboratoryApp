using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LaboratoryApp.src.Shared.Converters
{
    public class HtmlToDataUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Lấy chuỗi HTML từ ViewModel
            var html = value as string ?? "";
            // Mã hóa base64 (đảm bảo ký tự an toàn)
            var bytes = Encoding.UTF8.GetBytes(html);
            var base64 = System.Convert.ToBase64String(bytes);
            // Tạo Data URI
            var uri = $"data:text/html;base64,{base64}";
            return new Uri(uri);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
