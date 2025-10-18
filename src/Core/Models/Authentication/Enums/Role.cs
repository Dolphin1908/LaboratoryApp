using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authentication.Enums
{
    [Flags]
    public enum Role
    {
        [Display(Name = "khách")]
        Guest = 1, // Khách
        [Display(Name = "học viên")]
        Student = 2, // Học viên
        [Display(Name = "giảng viên")]
        Instructor = 4, // Giảng viên
        [Display(Name = "quản trị viên")]
        Admin = 8 // Quản trị viên
    }
}
