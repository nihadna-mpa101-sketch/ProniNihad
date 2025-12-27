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
            builder.Services.AddDbContext<AddDbContext>(options =>
            {
                  options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            var app = builder.Build();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
