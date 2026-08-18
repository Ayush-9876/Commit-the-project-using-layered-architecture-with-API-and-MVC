using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Profile.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30); 
    options.Cookie.HttpOnly = true;                 
    options.Cookie.IsEssential = true;              
});
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/ProfileMVC/Login";
        options.AccessDeniedPath = "/ProfileMVC/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
var app = builder.Build();

app.UseSession();
app.MapControllers();
app.Run();
