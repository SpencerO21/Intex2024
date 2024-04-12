using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Intex2024.Data;

using Microsoft.Extensions.DependencyInjection;

using Intex2024.Models;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("IdentityConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// SQL Server Database Configuration
var storeConnectionString = builder.Configuration.GetConnectionString("StoreConnection") ??
                            throw new InvalidOperationException("Connection string 'StoreConnection' not found.");
builder.Services.AddDbContext<IntexStoreContext>(options =>
    options.UseSqlServer(storeConnectionString));
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

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
app.UseSession();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers.Add("Content-Security-Policy",
    "default-src 'self'; img-src 'self' *;");
    await next();
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "paginationWithPageSize",
    pattern: "Products/{color}/{cat}/{pageNum}/{pageSize?}", // Adding pageSize as an optional parameter
    defaults: new { Controller = "Home", action = "Index"});

app.MapControllerRoute(
    name: "productByCategoryWithPageSize",
    pattern: "Category/{cat}/{pageNum}/{pageSize?}", // Adding pageSize as an optional parameter
    defaults: new { Controller = "Home", action = "Index"});

app.MapControllerRoute(
    name: "productByColorWithPageSize",
    pattern: "Color/{color}/{pageNum}/{pageSize?}", // Adding pageSize as an optional parameter
    defaults: new { Controller = "Home", action = "Index"});

app.MapControllerRoute(
    name: "paginationProductsAdmin",
    pattern: "/Page/{pageNum}",
    defaults: new { Controller = "Admin", action = "ViewProducts", pageNum = 1 });

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page/{pageNum}", // Ensure this doesn't conflict with other patterns
    defaults: new { Controller = "Home", action = "Index", pageNum = 1 });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();


app.Run();