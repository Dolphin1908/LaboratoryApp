using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authentication
{
    public class Group : BaseAuthentication
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public long OwnerId { get; set; } // User who created the group

        public List<long> Members { get; set; } = new();
    }
}
