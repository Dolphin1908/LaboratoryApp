using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry.Enums
{
    public enum SubstanceKind
    {
        [Display(Name = "Đơn chất")]Element,
        [Display(Name = "Hợp chất")] Compound
    }
}
