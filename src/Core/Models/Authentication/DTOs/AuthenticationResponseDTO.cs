using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authentication.DTOs
{
    public class AuthenticationResponseDTO
    {
        public UserDTO User { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
    }
}
