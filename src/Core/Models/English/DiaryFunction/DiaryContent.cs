using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.English.DiaryFunction
{
    public class DiaryContent
    {
        #region DiaryInfo
        public long Id { get; set; }
        public string Title { get; set; }
        public string Mode { get; set; }

        // Lưu nội dung FlowDocument dạng base64 string
        public byte[] ContentBytes { get; set; } = Array.Empty<byte>();

        // Thông tin phụ
        public string ContentFormat { get; set; } = "XamlPackage"; // Hoặc "Rtf", "Text"
        #endregion

        #region WriterInfo
        public long UserId { get; set; }
        #endregion

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
