using Microsoft.AspNetCore.Authentication.Cookies;
using sellphone.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Đăng ký HttpClient gọi sang Backend 
builder.Services.AddScoped(sp =>
{
    // 1. Tạo bộ xử lý để bỏ qua lỗi SSL 
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    };

    // 2. Tạo HttpClient và GẮN ĐỊA CHỈ API 
    return new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:5154/")
    };
});

// 3. Cấu hình Authentication (Cookie)
builder.Services.AddCascadingAuthenticationState(); 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "SellphoneAuth";
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// 4. Add Controller 
builder.Services.AddControllers();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();

// 5. Thứ tự Middleware 
app.UseRouting();
app.UseAuthentication(); // Check vé
app.UseAuthorization();  // Check quyền
app.UseAntiforgery();    // Bảo mật Form

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map Controller để Form Login tìm thấy đường vào
app.MapControllers();

app.Run();