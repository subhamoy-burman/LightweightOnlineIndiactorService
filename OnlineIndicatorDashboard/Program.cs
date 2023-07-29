using OnlineIndicatorDashboard.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.ResponseCaching;

var builder = WebApplication.CreateBuilder(args);

// Add controllers.
builder.Services.AddControllersWithViews();
var app = builder.Build();


// Register controllers
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.UseEndpoints(endpoints => {
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//});

app.Run();
