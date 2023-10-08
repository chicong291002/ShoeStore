using ApiIntegration.Slides;
using FluentValidation.AspNetCore;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ShoeStore.AdminApp.ApiIntegration.Products;
using SmartPhoneStore.AdminApp.ApiIntegration.Categories;
using SmartPhoneStore.AdminApp.ApiIntegration.Products;
using SmartPhoneStore.AdminApp.ApiIntegration.Users;
using SmartPhoneStore.ViewModels.System.Users.CheckUserValidator;
using SmartPhoneStore.WebApp.LocalizationResources;
using System;
using System.Configuration;
using System.Globalization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var cultures = new[]
{
                new CultureInfo("en"),
                new CultureInfo("vi"),
            };
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; // neu chua login thi Redirect ve day 
        options.AccessDeniedPath = "/User/Forbidden/";
    });

builder.Services.AddControllersWithViews()
     .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>())
                .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
                {
                    ops.UseAllCultureProviders = false;
                    ops.ResourcesPath = "LocalizationResources";
                    ops.RequestLocalizationOptions = o =>
                    {
                        o.SupportedCultures = cultures;
                        o.SupportedUICultures = cultures;
                        o.DefaultRequestCulture = new RequestCulture("vi");
                    };
                }); ;
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ISlideApiClient, SlideApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IOrderApiClient, OrderApiClient>();

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

app.UseAuthentication();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
                    name: "Product Category En",
                    pattern: "{culture}/categories/{id}", new
                    {
                        controller = "Product",
                        action = "Category"
                    });

app.MapControllerRoute(
  name: "Product Category Vn",
  pattern: "{culture}/danh-muc/{id}", new
  {
      controller = "Product",
      action = "Category"
  });

app.MapControllerRoute(
    name: "Product Detail En",
    pattern: "{culture}/products/{id}", new
    {
        controller = "Product",
        action = "Detail"
    });

app.MapControllerRoute(
  name: "Product Detail Vn",
  pattern: "{culture}/san-pham/{id}", new
  {
      controller = "Product",
      action = "Detail"
  });

app.MapControllerRoute(
     name: "default",
     pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
app.Run();
