using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Assignment.Enums
{
    /// <summary>
    /// Độ khó của bài tập
    /// </summary>
    public enum DifficultyLevel
    {
        [Display(Name = "Dễ")]         
        Easy,    // Mới bắt đầu

        [Display(Name = "Trung bình")] 
        Medium,  // Đã có kiến thức cơ bản

        [Display(Name = "Khó")]        
        Hard,    // Nâng cao

        [Display(Name = "Rất khó")]
        Insane   // Chuyên sâu
    }
}
