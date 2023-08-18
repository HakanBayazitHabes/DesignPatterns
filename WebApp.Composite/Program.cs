using BaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
    .AddEntityFrameworkStores<AppIdentityDbContext>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using var scope = app.Services.CreateScope(); //.ServiceProvider.GetRequiredService<AppIdentityDbContext>().Database.Migrate();

var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

identityDbContext.Database.Migrate();

if (!userManager.Users.Any())
{
    userManager.CreateAsync(new AppUser() { UserName = "user1", Email = "user1@gmail.com" }, "Password12*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user2", Email = "user2@gmail.com" }, "Password12*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user3", Email = "user3@gmail.com" }, "Password12*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user4", Email = "user4@gmail.com" }, "Password12*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user5", Email = "user5@gmail.com" }, "Password12*").Wait();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
