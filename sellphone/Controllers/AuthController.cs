using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using MyWebApp.Repositories;
using System.Security.Claims;

namespace MyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public AuthController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                // 1. Kiểm tra kỹ dữ liệu đầu vào
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                    return BadRequest("Vui lòng nhập đầy đủ thông tin.");

                if (await _userRepo.EmailExistsAsync(model.Email))
                    return BadRequest("Email này đã tồn tại, vui lòng chọn email khác.");

                var newUser = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = "Customer",
                    CreatedAt = DateTime.Now
                };

                // 2. Lưu vào DB (Có await)
                await _userRepo.AddUserAsync(newUser);

                return Ok(new { message = "Đăng ký thành công" });
            }
            catch (Exception ex)
            {
                // Bắt lỗi hệ thống để biết tại sao không lưu được
                return StatusCode(500, "Lỗi Server: " + ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var user = await _userRepo.GetUserByEmailAsync(model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return BadRequest("Tài khoản hoặc mật khẩu không đúng.");
            }

            // --- TẠO COOKIE ---
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("UserId", user.Id.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            // Lệnh này tạo Cookie mã hóa gửi về trình duyệt
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("/login?error=Tài khoản hoặc mật khẩu không đúng");
        }
        [HttpGet("logout")]
        public async Task<IActionResult> LogoutPage()
        {
            // 1. Xóa Cookie xác thực
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 2. Chuyển hướng người dùng về trang Login
            return Redirect("/login");
        }
    }
}