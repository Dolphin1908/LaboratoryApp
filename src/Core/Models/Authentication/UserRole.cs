using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authentication
{
    public class UserRole
    {
        public User User { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }
    }
}
