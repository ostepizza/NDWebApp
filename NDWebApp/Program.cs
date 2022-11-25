using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NDWebApp.Data;
using NDWebApp.Areas.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("NDWebAppContextConnection") ?? throw new InvalidOperationException("Connection string 'NDWebAppContextConnection' not found.");

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
var reqDigit = settings.RequireDigit;
var reqLowercase = settings.RequireLowercase;
var reqNonAlphanumeric = settings.RequireNonAlphanumeric;
var reqUppercase = settings.RequireUppercase;
var reqLength = settings.RequiredLength;
var reqUniqueChars = settings.RequiredUniqueChars;
var reqConfirmedAccount = settings.RequireConfirmedAccount;

builder.Services.AddDbContext<NDWebAppContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 4, 25))));

builder.Services.AddDefaultIdentity<NDWebAppUser>(options => options.SignIn.RequireConfirmedAccount = reqConfirmedAccount)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NDWebAppContext>();

// Add Interfaces
builder.Services.AddScoped<ISqlConnector, TeamSqlConnector>();
builder.Services.AddScoped<ISuggestionConnector, SuggestionSqlConnector>();
builder.Services.AddScoped<IRepairsSqlConnector, RepairsSqlConnector>();
builder.Services.AddScoped<IUsersSqlConnector, UsersSqlConnector>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = reqDigit;
    options.Password.RequireLowercase = reqLowercase;
    options.Password.RequireNonAlphanumeric = reqNonAlphanumeric;
    options.Password.RequireUppercase = reqUppercase;
    options.Password.RequiredLength = reqLength;
    options.Password.RequiredUniqueChars = reqUniqueChars;
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.Initialize(scope.ServiceProvider);
}

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

//app.MapControllerRoute(
//    name: "Admin",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
        name: "adminArea",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
