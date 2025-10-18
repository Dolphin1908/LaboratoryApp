using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authorization.Enums
{
    [Flags]
    public enum AccessLevel
    {
        [Display(Name = "Xem")] 
        View = 1, // Xem
        [Display(Name = "Làm bài")]
        Attempt = 2, // Thực hành
        [Display(Name = "Chấm điểm")]
        Grade = 4, // Chấm điểm
        [Display(Name = "Chỉnh sửa")]
        Edit = 8, // Chỉnh sửa
        [Display(Name = "Chủ sở hữu")]
        Owner = 16 // Chủ sở hữu
    }
}
