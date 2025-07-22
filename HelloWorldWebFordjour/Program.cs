using HelloWorldWebFordjour.Data; 
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HelloWorldWebFordjour.Interfaces; 
using HelloWorldWebFordjour.Repositories; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Adds MVC services

// --- Add Session services ---
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set a timeout for the session
    options.Cookie.HttpOnly = true; // Make the session cookie inaccessible to client-side script
    options.Cookie.IsEssential = true; // Make the session cookie essential for the app
});

// Register the DbContext with the dependency injection container
builder.Services.AddDbContext<TicketsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketsDbContext")));

// Register the Ticket Repository
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Enables serving static files from wwwroot

app.UseSession(); // Enables session capabilities

app.UseRouting(); // Enables routing capabilities

// --- Routing Configuration ---

app.MapControllerRoute(
    name: "static",
    pattern: "{controller=Home}/{action}/Page/{num}"); 

// Custom Routing Rule for "Assignments" dropdown
app.MapControllerRoute(
        name: "assignment6_1",
        pattern: "Assignment6_1/Index/{accessLevel:int}", // Explicitly map accessLevel as an integer
        defaults: new { controller = "Assignment6_1", action = "Index" }
    );

app.MapControllerRoute(
    name: "dataTransfer",
    pattern: "DataTransfer/{action}/{id?}",
    defaults: new { controller = "DataTransfer", action = "Index" });
//pattern: "Country/{gameFilter?}/{categoryFilter?}",
//defaults: new { controller = "Country", action = "Index" }

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default route: Controller/Action/OptionalId

app.Run();
