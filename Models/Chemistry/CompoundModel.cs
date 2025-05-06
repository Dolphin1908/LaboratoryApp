using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class CompoundModel
    {
        public long Id { get; set; } // ID của hợp chất
        public string Name { get; set; } // Tên hợp chất
        public string Formula { get; set; } // Công thức hóa học
        public double MolecularMass { get; set; } // Đơn vị: g/mol
        public string Phase { get; set; }          // Ví dụ: chất rắn, lỏng, khí
        public double? Density { get; set; }       // Đơn vị: g/cm³ (tùy chọn)
        public string CASNumber { get; set; }      // Số đăng ký hóa chất (tùy chọn)
        public List<CompoundElement> Composition { get; set; } // List of elements and their quantities
    }

    public class CompoundElement
    {
        public ElementModel Element { get; set; } // Phần tử hóa học
        public int Quantity { get; set; } // Số lượng nguyên tử của phần tử trong hợp chất
    }

    public class ChemicalProperty
    {
        public long Id { get; set; }
        public string PropertyName { get; set; } // Ví dụ: "Tính oxy hóa", "Phản ứng với axit"
        public string Description { get; set; }  // Mô tả cụ thể
        public long CompoundId { get; set; }
        public CompoundModel Compound { get; set; }
    }

    public class PhysicalProperty
    {
        public long Id { get; set; }
        public string PropertyName { get; set; } // Ví dụ: "Nhiệt độ nóng chảy", "Độ tan"
        public string Unit { get; set; }         // Đơn vị: °C, g/L,...
        public double Value { get; set; }
        public long CompoundId { get; set; }
        public CompoundModel Compound { get; set; }
    }

    public class CompoundNote
    {
        public long Id { get; set; }
        public string NoteType { get; set; }    // "An toàn", "Lưu trữ", "Ứng dụng", ...
        public string Content { get; set; }
        public long CompoundId { get; set; }
        public CompoundModel Compound { get; set; }
    }
}
