using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry.Enums
{
    public enum CompoundType
    {
        // Đơn chất (nguyên tố tồn tại ở trạng thái tự nhiên hoặc phân tử)
        [Display(Name = "Đơn chất kim loại")] ElementalMetal,          //0 Fe, Cu, Al
        [Display(Name = "Đơn chất phi kim")] ElementalNonMetal,        //1 O₂, H₂, Cl₂
        [Display(Name = "Khí hiếm")] NobleGas,                         //2 He, Ne, Ar

        // Hợp chất vô cơ
        [Display(Name = "Hợp chất vô cơ")] InorganicCompound,          //3 NaCl, CO₂, H₂SO₄
        [Display(Name = "Oxit")] Oxide,                                //4 CO₂, Fe₂O₃
        [Display(Name = "Axit")] Acid,                                 //5 HCl, H₂SO₄
        [Display(Name = "Bazơ")] Base,                                 //6 NaOH, NH₃
        [Display(Name = "Muối")] Salt,                                 //7 NaCl, K₂SO₄
        [Display(Name = "Hidroxit")] Hydroxide,                        //8 Ca(OH)₂
        [Display(Name = "Hợp chất phức")] ComplexCompound,             //9 [Cu(NH₃)₄]²⁺

        // Hợp chất hữu cơ
        [Display(Name = "Hợp chất hữu cơ")] OrganicCompound,           //10 CH₄, C₂H₅OH
        [Display(Name = "Polyme")] Polymer,                            //11 PVC, PE, Nylon

        // Đặc tính hóa học đặc biệt
        [Display(Name = "Chất lưỡng tính")] Amphoteric,                //12 Zn(OH)₂, Al(OH)₃

        // Vai trò
        [Display(Name = "Dung môi")] Solvent,                          //13 Nước, ethanol...

        // Loại khác
        [Display(Name = "Khác")] Other                                 //14
    }
}
