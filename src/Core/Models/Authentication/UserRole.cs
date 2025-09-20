using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authentication
{
    public class UserRole : BaseAuthentication
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
 