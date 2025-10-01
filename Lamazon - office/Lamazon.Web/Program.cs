using Lamazon.Services.Extensions;
using Lamazon.Services.Implementations;
using Lamazon.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.InjectDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.InjectRepositories();
builder.Services.InjectServices();
builder.Services.InjectAutoMapper();

builder.Services.AddHttpClient<IGeoTrackerService, GeoTrackerService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/User/Login";
        option.ExpireTimeSpan = TimeSpan.FromHours(1);
        option.SlidingExpiration = true; // Renew - reset the cookie on each request
        
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseDebugIpAddressMiddleware(); // Custom middleware to show the IP address in the response headers
// Enable authentication middleware - flow should be: UseRouting -> UseAuthentication -> UseAuthorization -> UseEndpoints
// This order is important
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
