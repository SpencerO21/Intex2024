using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Intex2024.Data;
using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// SQL Server Database Configuration
var storeConnectionString = builder.Configuration.GetConnectionString("StoreConnection") ??
                            throw new InvalidOperationException("Connection string 'StoreConnection' not found.");
builder.Services.AddDbContext<IntexStoreContext>(options =>
    options.UseSqlServer(storeConnectionString));
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "productDetails",
    pattern: "Products/{color}/{cat}/{pageNum}",
    defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "productByCategory",
    pattern: "Category/{cat}/{pageNum?}", // Use a distinct path segment to differentiate
    defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "productByColor",
    pattern: "Color/{color}/{pageNum?}", // Use a distinct path segment to differentiate
    defaults: new { Controller = "Home", action = "Index"});

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page/{pageNum}", // Ensure this doesn't conflict with other patterns
    defaults: new { Controller = "Home", action = "Index", pageNum = 1 });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();