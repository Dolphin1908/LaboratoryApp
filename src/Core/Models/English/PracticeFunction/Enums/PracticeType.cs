using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LaboratoryApp.src.Core.Models.English.PracticeFunction.Enums
{
    public enum PracticeType
    {
        [Display(Name = "Luyện tập")] Practice,
        [Display(Name = "Kiểm tra")] Test,
        [Display(Name = "Thi")] Exam
    }
}
