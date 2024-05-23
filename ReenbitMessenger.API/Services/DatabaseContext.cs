using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess.Data;

namespace ReenbitMessenger.API.Services
{
    public static class DatabaseContext
    {
        public static void AddDatabaseContext(this IServiceCollection services,
            ConfigurationManager config, IWebHostEnvironment environment)
        {
            string dbConnectionString = string.Empty;

            if (environment.IsDevelopment())
            {
                dbConnectionString = config.GetConnectionString("LocalDbConnection");
            }
            else
            {
                dbConnectionString = config.GetSection("MessengerDbConnection").Value;
            }

            services.AddDbContext<MessengerDataContext>(options =>
            {
                options.UseSqlServer(dbConnectionString);
            });
        }
    }
}
