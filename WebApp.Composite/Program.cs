using BaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Composite.Models;

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
    var newUser = new AppUser() { UserName = "user1", Email = "user1@gmail.com" };
    userManager.CreateAsync(newUser, "Password12*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user2", Email = "user2@gmail.com" }, "Password12*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user3", Email = "user3@gmail.com" }, "Password12*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user4", Email = "user4@gmail.com" }, "Password12*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user5", Email = "user5@gmail.com" }, "Password12*").Wait();

    var newCategory1 = new Category() { Name = "Suç Romanlarý", UserId = newUser.Id };
    var newCategory2 = new Category() { Name = "Cinayet Romanlarý", UserId = newUser.Id };
    var newCategory3 = new Category() { Name = "Polisiye Romanlarý", UserId = newUser.Id };

    identityDbContext.Categories.AddRange(newCategory1, newCategory2, newCategory3);

    identityDbContext.SaveChanges();

    var subCategory1 = new Category() { Name = "Suç Romanlar 1", UserId = newUser.Id, ReferenceId = newCategory1.Id };
    var subCategory2 = new Category() { Name = "Cinayet Romanlar 1", UserId = newUser.Id, ReferenceId = newCategory2.Id };
    var subCategory3 = new Category() { Name = "Polisiye Romanlar 1", UserId = newUser.Id, ReferenceId = newCategory3.Id };

    identityDbContext.Categories.AddRange(subCategory1, subCategory2, subCategory3);
    identityDbContext.SaveChanges();

    var subCategory4 = new Category() { Name = "Cinayet Romanlar 1.1", UserId = newUser.Id, ReferenceId = subCategory2.Id };

    identityDbContext.Categories.Add(subCategory4);
    identityDbContext.SaveChanges();




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
