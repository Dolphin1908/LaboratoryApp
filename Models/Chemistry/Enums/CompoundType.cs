using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.Chemistry.Enums
{
    public enum CompoundType
    {
        [Display(Name = "Hợp chất vô cơ")] InorganicCompound,      // 00 Ví dụ: NaCl, H₂SO₄
        [Display(Name = "Hợp chất hữu cơ")] OrganicCompound,       // 01 Ví dụ: CH₄, C₆H₆
        [Display(Name = "Axit")] Acid,                             // 02 HCl, H₂SO₄
        [Display(Name = "Bazơ")] Base,                             // 03 NaOH, NH₃
        [Display(Name = "Muối")] Salt,                             // 04 NaCl, K₂SO₄
        [Display(Name = "Oxit")] Oxide,                            // 05 CO₂, Fe₂O₃
        [Display(Name = "Hidroxit")] Hydroxide,                    // 06 Ca(OH)₂
        [Display(Name = "Hợp chất phức")] ComplexCompound,         // 07 [Cu(NH₃)₄]²⁺
        [Display(Name = "Polyme")] Polymer,                        // 08 PVC, PE
        [Display(Name = "Chất lưỡng tính")] Amphoteric,            // 09 Vừa là acid, vừa là base
        [Display(Name = "Dung môi")] Solvent,                      // 10 Dùng làm dung môi phổ biến
        [Display(Name = "Khác")] Other                             // 11
    }
}
