// ============================================================================
// === File: Program.cs
// === Description: Entry point of the Klemmbausteine web application. 
// ===              Configures services, database connection, middleware, 
// ===              and routing for the ASP.NET Core MVC architecture.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Klemmbausteine.Data;

namespace Klemmbausteine
{
    public class Program
    {
        // --- Main entry point of the application ---
        public static void Main(string[] args)
        {
            // --- Create and configure the web application builder ---
            var builder = WebApplication.CreateBuilder(args);

            // --- Register the database context using SQL Server and connection string from appsettings.json ---
            builder.Services.AddDbContext<KlemmbausteineContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("KlemmbausteineContext")
                    ?? throw new InvalidOperationException("Connection string 'KlemmbausteineContext' not found.")));

            // --- Add support for MVC controllers and views ---
            builder.Services.AddControllersWithViews();

            // --- Build the configured web application ---
            var app = builder.Build();

            // --- Configure the HTTP request pipeline ---
            if (!app.Environment.IsDevelopment())
            {
                // --- Production mode: use custom error handler and enable HSTS ---
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // --- Enforce HTTPS and enable routing ---
            app.UseHttpsRedirection();
            app.UseRouting();

            // --- Enable authorization middleware ---
            app.UseAuthorization();

            // --- Map static files and MVC controller routes ---
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Products}/{action=Index}/{id?}")
                .WithStaticAssets();

            // --- Run the web application ---
            app.Run();
        }
    }
}
