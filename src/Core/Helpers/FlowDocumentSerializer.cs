using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;

namespace LaboratoryApp.src.Core.Helpers
{
    public static class FlowDocumentSerializer
    {
        /// <summary>
        /// Lưu FlowDocument thành chuỗi thường
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string SerializeToString(FlowDocument document)
        {
            // Do nothing
            return "nothing";
        }

        /// <summary>
        /// Tạo FlowDocument từ chuỗi thường
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static FlowDocument DeserializeFromString (string str)
        {
            var doc = new FlowDocument();
            if (string.IsNullOrEmpty(str)) return doc;
            try
            {
                doc.Blocks.Add(new Paragraph(new Run(str)));
                return doc;
            }
            catch
            {
                // Nếu giải mã thất bại, trả về FlowDocument trống
                return new FlowDocument();
            }
        }

        /// <summary>
        /// Lưu FlowDocument thành chuỗi Base64
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string SerializeToBase64(FlowDocument document)
        {
            if (document == null) return string.Empty;
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            using var ms = new MemoryStream();
            range.Save(ms, DataFormats.XamlPackage);
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Tạo FlowDocument từ chuỗi Base64
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static FlowDocument DeserializeFromBase64(string base64String)
        {
            var doc = new FlowDocument();
            if (string.IsNullOrEmpty(base64String)) return doc;
            try
            {
                var bytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(bytes);
                var range = new TextRange(doc.ContentStart, doc.ContentEnd);
                range.Load(ms, DataFormats.XamlPackage);
                return doc;
            }
            catch
            {
                // Nếu giải mã thất bại, trả về FlowDocument trống
                return new FlowDocument();
            }
        }

        /// <summary>
        /// Lưu FlowDocument thành mảng byte
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(FlowDocument document)
        {
            if(document == null) return Array.Empty<byte>();
            var range = new TextRange(document.ContentStart, document.ContentEnd);
            using var ms = new MemoryStream();
            range.Save(ms, DataFormats.XamlPackage);
            return ms.ToArray();
        }

        /// <summary>
        /// Tạo FlowDocument từ mảng byte
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static FlowDocument DeserializeFromBytes(byte[] bytes)
        {
            var document = new FlowDocument();
            if (bytes == null || bytes.Length == 0) return document;
            try
            {
                using var ms = new MemoryStream(bytes);
                var range = new TextRange(document.ContentStart, document.ContentEnd);
                range.Load(ms, DataFormats.XamlPackage);
                return document;
            }
            catch
            {
                // Nếu giải mã thất bại, trả về FlowDocument trống
                return new FlowDocument();
            }
        }
    }
}
