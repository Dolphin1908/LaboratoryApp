using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry.Enums
{
    public enum ChemicalPhase
    {
        [Display(Name = "Rắn")]
        Solid,

        [Display(Name = "Lỏng")]
        Liquid,

        [Display(Name = "Khí")]
        Gas,

        [Display(Name = "Dung dịch")]
        Aqueous,

        [Display(Name = "Plasma")]
        Plasma, // (tuỳ chọn mở rộng)

        [Display(Name = "Siêu tới hạn")]
        Supercritical // (tuỳ chọn mở rộng)
    }

}
