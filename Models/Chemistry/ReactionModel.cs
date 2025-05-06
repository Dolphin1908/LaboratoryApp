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
}
