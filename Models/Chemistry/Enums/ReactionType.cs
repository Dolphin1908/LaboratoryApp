using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry.Enums
{
    public enum ReactionType
    {
        [Display(Name = "Phản ứng tổng hợp")] Synthesis,              // A + B → AB
        [Display(Name = "Phản ứng phân hủy")] Decomposition,         // AB → A + B
        [Display(Name = "Phản ứng thế")] SingleDisplacement,         // A + BC → AC + B
        [Display(Name = "Phản ứng hai chất thế")] DoubleDisplacement,// AB + CD → AD + CB
        [Display(Name = "Phản ứng cháy")] Combustion,                // CH₄ + O₂ → CO₂ + H₂O
        [Display(Name = "Phản ứng oxi hóa khử")] Redox,              // Gồm sự trao đổi electron
        [Display(Name = "Phản ứng axit-bazơ")] AcidBase,             // HCl + NaOH → NaCl + H₂O
        [Display(Name = "Phản ứng kết tủa")] Precipitation,          // Tạo chất rắn không tan
        [Display(Name = "Phản ứng tạo phức chất")] ComplexFormation,// Phức chất như [Cu(NH₃)₄]²⁺
        [Display(Name = "Khác")] Other
    }
}
