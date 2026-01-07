using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models
{
    public class User
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Mật khẩu đã mã hóa
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}