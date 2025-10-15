using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry.Common.Enums
{
    public enum ReactionNoteType
    {
        [Display(Name = "Cơ chế phản ứng")] Mechanism,            // Giải thích từng bước phản ứng
        [Display(Name = "Điều kiện phản ứng")] Condition,         // Nhiệt độ, xúc tác, áp suất,...
        [Display(Name = "Quan sát thí nghiệm")] Observation,      // Màu sắc, kết tủa, mùi,...
        [Display(Name = "Giải thích sản phẩm")] ProductRationale, // Tại sao sản phẩm lại như vậy
        [Display(Name = "Ứng dụng")] Application,                 // Ứng dụng thực tiễn của phản ứng
        [Display(Name = "Tính toán")] Calculation,                // Tính hiệu suất, nồng độ,...
        [Display(Name = "Tham khảo")] Reference,                  // Nguồn tài liệu, sách,...
        [Display(Name = "An toàn")] Safety,                       // Biện pháp an toàn khi thực hiện phản ứng
        [Display(Name = "Khác")] Other
    }
}
