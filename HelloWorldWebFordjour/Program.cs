using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Adds MVC services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Enables serving static files from wwwroot

app.UseRouting(); // Enables routing capabilities

// --- Routing Configuration ---

app.MapControllerRoute(
    name: "static",
    pattern: "{controller=Home}/{action}/Page/{num}"); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default route: Controller/Action/OptionalId

// Custom Routing Rule for "Organize Assignments"
app.MapControllerRoute(
    name: "organizeAssignments",
    pattern: "assignments/organize",
    defaults: new { controller = "Assignments", action = "Organize" });

app.Run();
