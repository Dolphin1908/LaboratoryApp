using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Helpers
{
    public static class OtpHelper
    {
        public static string GenerateOtp(int length = 6)
        {
            var random = new Random();
            var otp = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                otp.Append(random.Next(0, 10)); // Append a random digit (0-9)
            }
            return otp.ToString();
        }
    }
}
