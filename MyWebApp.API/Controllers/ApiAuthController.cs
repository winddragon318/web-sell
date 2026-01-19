using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.API.Repositories; // ✅ Dùng Repo của API
using MyWebApp.Shared.Models;

namespace MyWebApp.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public AuthApiController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // 1. Logic Đăng ký
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Logic kiểm tra
            if (await _userRepo.EmailExistsAsync(model.Email))
                return BadRequest("Email đã tồn tại.");

            // Logic tạo user & mã hóa mật khẩu
            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password), 
                Role = "Customer",
                CreatedAt = DateTime.Now
            };

            // Lưu xuống DB
            await _userRepo.AddUserAsync(newUser);

            return Ok(new { message = "Đăng ký thành công" });
        }

        // 2. Logic Check Đăng nhập 
        [HttpPost("check-login")]
        public async Task<IActionResult> CheckLogin([FromBody] LoginModel model)
        {
            // Tìm user
            var user = await _userRepo.GetUserByEmailAsync(model.Email);

            // Kiểm tra mật khẩu
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return BadRequest("Tài khoản hoặc mật khẩu không đúng.");
            }

            // Trả về thông tin User 
            // TUYỆT ĐỐI KHÔNG TRẢ VỀ PASSWORD
            return Ok(new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }
    }
}