using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry
{
    public class ReactionModel
    {
        public long Id { get; set; } // ID của phản ứng
        public List<ReactionComponent> Reactants { get; set; }      // Các chất tham gia phản ứng
        public List<ReactionComponent> Products { get; set; }       // Các sản phẩm của phản ứng
        public double? YieldPercent { get; set; }     // Hiệu suất phản ứng (%) (tùy chọn)
        public string Conditions { get; set; }        // Điều kiện phản ứng (nhiệt độ, áp suất, pH, v.v.)
        public DateTime? ReactionDate { get; set; }   // Ngày thực hiện phản ứng (tùy chọn)
        public string Notes { get; set; }         // Ghi chú về phản ứng (tùy chọn)
    }

    public class ReactionComponent
    {
        public CompoundModel Compound { get; set; } // Hợp chất tham gia phản ứng
        public int StoichiometricCoefficient { get; set; } // Hệ số tỉ lệ của hợp chất trong phản ứng
    }

    public class ReactionCondition
    {
        public long Id { get; set; }
        public string Temperature { get; set; } // VD: "25°C", hoặc phạm vi: "50–70°C"
        public string Pressure { get; set; }    // VD: "1 atm", "2 bar"
        public string Catalyst { get; set; }    // VD: "Pt", "H2SO4"
        public string Solvent { get; set; }     // VD: "H2O", "EtOH"
        public string PH { get; set; }          // VD: "7", "2–4"
        public string OtherConditions { get; set; } // Mô tả thêm (nếu cần)

        // Nếu bạn muốn liên kết với phản ứng cụ thể:
        public long ReactionId { get; set; }
        public ReactionModel Reaction { get; set; }
    }

    public class ReactionNote
    {
        public long Id { get; set; }
        public string Title { get; set; }       // VD: "Chú ý", "Mô tả thêm"
        public string Content { get; set; }
        public long ReactionId { get; set; }
        public ReactionModel Reaction { get; set; }
    }

}
