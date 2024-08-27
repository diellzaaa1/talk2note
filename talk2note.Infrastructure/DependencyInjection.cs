using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using talk2note.Application.Interfaces;
using talk2note.Application.Services;
using talk2note.Infrastructure.Data;
using talk2note.Infrastructure.Persistence;
using talk2note.Infrastructure.Repositories;


namespace talk2note.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("Server=localhost;Database=talk2note;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;",
                    b => b.MigrationsAssembly("talk2note.API")); 
            });
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
           services.AddScoped<IUserRepository, UserRepository>();





            return services;
        }
    }
}
