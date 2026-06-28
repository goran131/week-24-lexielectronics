using LexiElectronics.Data;
using LexiElectronics.Models;
using LexiElectronics.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LexiElectronics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
           
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddSession(options => { 
                options.IdleTimeout = TimeSpan.FromMinutes(60); 
                options.Cookie.HttpOnly = true; options.Cookie.IsEssential = true; 
            });
            
            builder.Services.AddRazorPages();

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

            app.UseSession();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            // This method only runs once when the database is empty.
            var result = RunMigrationsAndAddData(app);

            app.Run();
        }

        private static async Task<IActionResult> RunMigrationsAndAddData(WebApplication app) { 
            using (var scope = app.Services.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!appDbContext.Users.Any())
                {
                    appDbContext.Database.Migrate();

<<<<<<< HEAD
                    var scriptPath = "./Arkiv/LexiElectronics.data.sql";
=======
                    var scriptPath = Path.Combine(AppContext.BaseDirectory, "Arkiv", "LexiElectronics.data.sql");
>>>>>>> 9afb6b5244185e01eb1bdc943dd83e37ff363f07
                    var sqlScript = File.ReadAllText(scriptPath);

                    using (var connection = appDbContext.Database.GetDbConnection())
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = sqlScript;
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }

            return null;
        }
    }
}

