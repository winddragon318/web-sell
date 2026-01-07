using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Middleware;
using MyWebApp.Repositories;
using sellphone.Components;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ================== 1. ĐĂNG KÝ SERVICES (DI CONTAINER) ==================

// 1. Controller & Blazor
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Kích hoạt tính năng "Nhận diện người dùng" cho Blazor
builder.Services.AddCascadingAuthenticationState();

// 3. Cấu hình Authentication & Cookie (GỘP LÀM 1 LẦN DUY NHẤT Ở ĐÂY)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "MyAppCookie"; // Tên Cookie
        options.LoginPath = "/login";        // Trang đăng nhập
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Hết hạn sau 30p
        options.SlidingExpiration = true;    // Tự gia hạn nếu người dùng còn thao tác
    });

// 4. Repository & DB
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 5. HttpClient (Cấu hình Port)
builder.Services.AddScoped(sp =>
{
    var handler = new HttpClientHandler();
    if (builder.Environment.IsDevelopment())
    {
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    }
    var client = new HttpClient(handler);
    // 👇 Kiểm tra kỹ port này trong launchSettings.json nhé
    client.BaseAddress = new Uri("https://localhost:7033/");
    return client;
});

var app = builder.Build();

// ================== 2. CẤU HÌNH PIPELINE (MIDDLEWARE) ==================
// ⚠️ THỨ TỰ CÁC DÒNG DƯỚI ĐÂY LÀ BẮT BUỘC ĐÚNG ⚠️

// 1. Xử lý lỗi
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// 2. Load file tĩnh (CSS/JS/Ảnh) - Phải load được ảnh trước khi check quyền
app.UseStaticFiles();

// 3. Định tuyến (Tìm đường đi trước)
app.UseRouting();

// 4. Xác thực & Phân quyền (Check vé sau khi biết đường đi)
app.UseAuthentication(); // Bạn là ai?
app.UseAuthorization();  // Bạn được phép vào không?

// 5. Chống giả mạo (Blazor bắt buộc cái này nằm sau Auth)
app.UseAntiforgery();

// 6. Map Endpoint
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();