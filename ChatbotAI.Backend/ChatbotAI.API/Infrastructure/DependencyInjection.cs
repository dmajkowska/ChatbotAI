using ChatbotAI.API.Domain.Interfaces.Repositories;
using ChatbotAI.API.Infrastructure.Persistance;
using ChatbotAI.API.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatbotDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IChatbotRepository), typeof(ChatbotRepository));

            return services;
        }
    }
}
