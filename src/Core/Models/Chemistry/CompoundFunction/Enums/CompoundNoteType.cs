using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Chemistry.Enums
{
    public enum CompoundNoteType
    {
        [Display(Name = "An toàn")] Safety,
        [Display(Name = "Bảo quản")] Storage,
        [Display(Name = "Sử dụng")] Usage,
        [Display(Name = "Tác dụng phụ")] SideEffect,
        [Display(Name = "Tham khảo")] SourceReference,
        [Display(Name = "Quan sát")] Observation,
        [Display(Name = "Khác")] Miscellaneous
    }
}
