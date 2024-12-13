using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using Version2.Models;
using Version2.Services;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HeThongBanSachContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình dịch vụ cho ứng dụng
builder.Services.AddControllersWithViews();

// Cấu hình Session
builder.Services.AddDistributedMemoryCache(); // Dùng bộ nhớ để lưu trữ session
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Chỉ có thể truy cập session từ máy chủ
    options.Cookie.IsEssential = true; // Chắc chắn rằng cookie là cần thiết cho hoạt động của ứng dụng
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Thời gian hết hạn của session
});

//Connect VNPay API
builder.Services.AddScoped<IVnPayService, VnPayService>();

var app = builder.Build();

// Cấu hình đường dẫn của các yêu cầu HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Cấu hình ứng dụng sử dụng session
app.UseSession();  // Quan trọng để ứng dụng sử dụng session

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index" +
    "}/{id?}");

app.Run();
