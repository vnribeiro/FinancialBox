using FinancialBox.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string DatabaseName = "DefaultConnection";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString(DatabaseName)));

            return services;
        }
    }
}