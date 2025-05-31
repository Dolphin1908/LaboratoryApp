using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authentication
{
    public class RefreshToken
    {
        public long Id { get; set; } // Id của Token
        public string Token { get; set; } = null!; // Token được sử dụng để xác thực lại người dùng
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Thời gian tạo token
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7); // Thời gian hết hạn token
        public DateTime? RevokedAt { get; set; } // Thời gian token bị thu hồi
        public string ReplacedBy { get; set; } = null!; // Token mới thay thế token này
        public User User { get; set; } = null!;
    }
}
