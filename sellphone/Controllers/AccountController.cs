using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Shared.Models; 
using System.Security.Claims;

namespace sellphone.Controllers 
{
    // Route này tạo ra đường dẫn /auth/...
    [Route("auth")]
    public class AccountController : Controller
    {
        private readonly HttpClient _http;

        // Sử dụng IHttpClientFactory để lấy cấu hình "BackendAPI" từ Program.cs
        public AccountController(HttpClient http)
        {
            _http = http; // Nó tự lấy cái có BaseAddress chuẩn luôn
        }

        // 1. XỬ LÝ ĐĂNG NHẬP
        // Đường dẫn gọi vào sẽ là: /auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            try
            {                
                // Endpoint bên API phải là: [HttpPost("check-login")]
                var response = await _http.PostAsJsonAsync("api/auth/check-login", model);

                // Bước 2: Nếu API bảo sai (400 Bad Request)
                if (!response.IsSuccessStatusCode)
                {
                    string thongBao = "Tài khoản hoặc mật khẩu không đúng";
                    string thongBaoDaMaHoa = System.Net.WebUtility.UrlEncode(thongBao);
                    // Quay về trang Login kèm thông báo lỗi
                    return Redirect("/login?error=" + thongBaoDaMaHoa);
                }

                // Bước 3: Nếu API bảo đúng (200 OK) -> Đọc thông tin User trả về
                var user = await response.Content.ReadFromJsonAsync<User>();

                if (user == null) return Redirect("/login?error=Lỗi dữ liệu từ Server");

                // Bước 4: TẠO COOKIE TẠI FRONTEND (Quan trọng nhất)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, System.Net.WebUtility.UrlEncode(user.Name)),

                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserId", user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Ghi Cookie vào trình duyệt (Giữ đăng nhập 60 phút)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                    });

                // Bước 5: Đăng nhập xong thì về trang chủ
                return Redirect("/");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi Server: " + ex.Message + " | Chi tiết: " + ex.StackTrace);
            }
        }

        // 2. XỬ LÝ ĐĂNG XUẤT
        // Đường dẫn gọi vào sẽ là: /auth/logout
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            // Xóa Cookie khỏi trình duyệt
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Quay về trang Login
            return Redirect("/login");
        }
    }
}