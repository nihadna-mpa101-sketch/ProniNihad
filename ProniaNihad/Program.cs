using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ProniaWebNihad
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                  options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            
            var app = builder.Build();
            
            app.UseStaticFiles();
            
            app.UseRouting();

            
                app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}"
                );


            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
