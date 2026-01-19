using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Shared.Models;

public class User
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Mật khẩu đã mã hóa
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
