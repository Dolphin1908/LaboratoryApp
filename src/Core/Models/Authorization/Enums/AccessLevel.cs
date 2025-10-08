using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authorization.Enums
{
    public enum AccessLevel
    {
        [Display(Name = "Xem")] View, // Xem
        [Display(Name = "Làm bài")] Attempt, // Thực hành
        [Display(Name = "Chấm điểm")] Grade, // Chấm điểm
        [Display(Name = "Chỉnh sửa")] Edit, // Chỉnh sửa
        [Display(Name = "Chủ sở hữu")] Owner // Chủ sở hữu
    }
}
