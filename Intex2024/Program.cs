using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Intex2024.Data;
using Intex2024.Models;

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
builder.Services.AddScoped<ITransactionRepository, EFTransactionRepository>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/{pageNum}/{pageSize}",
    defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//bro idk what these do
app.MapControllerRoute("pagenumandtype", "{projectType}/Page{pageNum}", new { Controller = "Home", Action = "Index" });
app.MapControllerRoute("page", "Page/{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("projectType", "{projectType}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("pagination", "Projects/Page{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });



app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();