using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.English.ExerciseFunction.Enums
{
    public enum DifficultyLevel
    {
        [Display(Name = "Dễ")]         Easy,
        [Display(Name = "Trung bình")] Medium,
        [Display(Name = "Khó")]        Hard,
        [Display(Name = "Rất khó")]    Insane
    }
}
