using LaboratoryApp.src.Core.Models.Authentication.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authentication
{
    public class EmailVerification
    {
        public long Id { get; set; }
        public string OtpCode { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; } = false;

        public OtpType Type { get; set; }

        // Foreign Key
        public long UserId { get; set; }
        public User User { get; set; } = null!;

    }
}
