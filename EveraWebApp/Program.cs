using EveraWebApp.DataContext;
using EveraWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireDigit= true;
    opt.Password.RequireLowercase= false;
    opt.Password.RequireUppercase= true;
    opt.Password.RequiredLength= 8;
    opt.Password.RequiredUniqueChars= 3;
    opt.Password.RequireNonAlphanumeric= false;

    opt.User.RequireUniqueEmail= true;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<EveraDbContext>();

builder.Services.AddDbContext<EveraDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddSession(cfg=>cfg.IdleTimeout= TimeSpan.FromSeconds(5));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
