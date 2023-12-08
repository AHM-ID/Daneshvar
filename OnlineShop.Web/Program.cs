using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Core.Convertors;
using OnlineShop.Core.Services;
using OnlineShop.Core.Services.Interfaces;
using OnlineShop.DataLayer.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMvc();

#region Authentication 
builder.Services
    .AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });
#endregion

#region DB-Context
builder.Services.AddDbContext<OnlineShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineShopConnection"));
});
#endregion 

#region IoC
builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddTransient<IViewRenderService, RenderViewToString>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
