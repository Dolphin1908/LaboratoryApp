using LaboratoryApp.Domain.Interfaces.Services.Infrastructure;
using System.Windows.Documents;
using System.Windows.Markup;

namespace LaboratoryApp.src.Services.Infrastructure
{
    public class FormatConversionService : IFormatConversionService
    {
        #region String <-> Base64
        /// <summary>
        /// Chuyển chuỗi thường thành chuỗi Base64
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string SerializeStringToBase64(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Chuyển chuỗi Base64 thành chuỗi thường
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public string DeserializeStringFromBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String)) return string.Empty;
            try
            {
                var bytes = Convert.FromBase64String(base64String);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region String <-> ByteArray
        /// <summary>
        /// Chuyển chuỗi thường thành mảng byte
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public byte[] SerializeStringToByteArray(string str)
        {
            if (string.IsNullOrEmpty(str)) return Array.Empty<byte>();
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// Chuyển mảng byte thành chuỗi thường
        /// </summary>
        /// <param name="byteArrayString"></param>
        /// <returns></returns>
        public string DeserializeStringFromByteArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return string.Empty;
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        #endregion

        #region ByteArray <-> Base64
        /// <summary>
        /// Chuyển mảng byte thành chuỗi Base64
        /// </summary>
        /// <param name="byteArrayString"></param>
        /// <returns></returns>
        public byte[] SerializeByteArrayToBase64(string byteArrayString)
        {
            if (string.IsNullOrEmpty(byteArrayString)) return Array.Empty<byte>();
            try
            {
                return Convert.FromBase64String(byteArrayString);
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Chuyển chuỗi Base64 thành mảng byte
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public string DeserializeByteArrayFromBase64(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return string.Empty;
            return Convert.ToBase64String(bytes);
        }
        #endregion

        #region XAML <-> PlainText
        /// <summary>
        /// Chuyển chuỗi XAML thành PlainText
        /// </summary>
        /// <param name="xamlString"></param>
        /// <returns></returns>
        public string ConvertXamlToPlainText(string xamlString)
        {
            if (string.IsNullOrWhiteSpace(xamlString))
                return string.Empty;

            try
            {
                // 1. Parse chuỗi XAML thành một FlowDocument ảo (trong bộ nhớ)
                // Lưu ý: Chuỗi xamlString phải là một XAML hợp lệ (bắt đầu bằng <FlowDocument...>)
                var flowDocument = XamlReader.Parse(xamlString) as FlowDocument;

                if (flowDocument == null) return string.Empty;

                // 2. Dùng TextRange để lấy toàn bộ text ra (bỏ qua các thẻ format màu sắc, in đậm...)
                var textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);

                return textRange.Text;
            }
            catch
            {
                // Nếu parse lỗi (do xamlString hỏng), trả về chuỗi rỗng hoặc chính nó tùy nhu cầu
                return string.Empty;
            }
        }
        /// <summary>
        /// Chuyển chuỗi PlainText thành XAML
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string ConvertPlainTextToXaml(string plainText)
        {
            // 1. Tạo mới một FlowDocument chuẩn
            var doc = new FlowDocument();

            // 2. Tạo một Paragraph chứa đoạn text mới
            // Run là đơn vị nhỏ nhất chứa text trong WPF Document
            var paragraph = new Paragraph(new Run(plainText ?? string.Empty));

            // Tùy chọn: Set style mặc định cho paragraph này nếu muốn (ví dụ font size, font family...)
            // paragraph.FontSize = 14; 

            doc.Blocks.Add(paragraph);

            // 3. Chuyển FlowDocument này thành chuỗi XAML string để binding ngược lại View
            return XamlWriter.Save(doc);
        }
        #endregion
    }
}
