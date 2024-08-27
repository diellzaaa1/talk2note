using talk2note.Application;
using talk2note.Infrastructure;

namespace talk2note.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddApplicationDI()
                .AddInfrastructureDI();
          
            return services;
        }
    }
}
