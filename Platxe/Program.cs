using CLiCore;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

iCore.Username = builder.Configuration.GetConnectionString("Username");
iCore.Password = builder.Configuration.GetConnectionString("Password");
iCore.Server = builder.Configuration.GetConnectionString("Server");
iCore.Conn = "Data Source=" + iCore.Server + ";Initial Catalog=" + builder.Configuration.GetConnectionString("Database") + ";User ID=" + iCore.Username + ";Password=" + iCore.Password + ";Connection Timeout=500000";
iCore.iLL = new List<PLxLanguage>();
iCore.iLL = new CLiCore.PLxLanguage().GetList();

// Add services to the container.
builder.Services
.AddControllersWithViews()
.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
         .AddCookie(options =>
         {
             options.LoginPath = "/Account/Login";
             options.AccessDeniedPath = "/Home/AccessDenied";
             options.ExpireTimeSpan = TimeSpan.FromDays(1);
             options.Cookie.MaxAge = TimeSpan.FromDays(1);
         });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStatusCodePagesWithRedirects("/errors/{0}");
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseFastReport();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
      name: "Settings",
      pattern: "{area=Settings}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Accounting",
    pattern: "{area=Accounting}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Financial",
    pattern: "{area=Financial}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "FixedAssets",
    pattern: "{area=FixedAssets}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Inventory",
    pattern: "{area=Inventory}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Purchasing",
    pattern: "{area=Purchasing}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Sales",
    pattern: "{area=Sales}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Manager",
    pattern: "{area=Manager}/{controller=Home}/{action=Index}/{id?}");
 
app.MapControllerRoute(
    name: "HR",
    pattern: "{area=HR}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Payroll",
    pattern: "{area=Payroll}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Auditing",
    pattern: "{area=Auditing}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Dashboard",
    pattern: "{area=Dashboard}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Print",
    pattern: "{area=Print}/{controller=Home}/{action=Index}/{id?}");
 
 



app.Run();

