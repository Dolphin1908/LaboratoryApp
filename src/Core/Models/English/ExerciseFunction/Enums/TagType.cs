using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.English.ExerciseFunction.Enums
{
    public enum TagType
    {
        [Display(Name = "Bài tập về nhà")] Homework,    // Bài tập về nhà
        [Display(Name = "Kiểm tra ngắn")]  Quiz,        // Kiểm tra ngắn
        [Display(Name = "Bài thi")]        Exam,        // Kỳ thi
        [Display(Name = "Luyện tập")]      Practice,    // Luyện tập
        [Display(Name = "Ôn tập")]         Review,      // Ôn tập
        [Display(Name = "Nhóm")]           Group,       // Nhóm
        [Display(Name = "Cá nhân")]        Individual   // Cá nhân
    }
}
