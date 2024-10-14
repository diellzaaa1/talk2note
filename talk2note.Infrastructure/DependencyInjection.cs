using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using talk2note.Application.Interfaces;
using talk2note.Application.Services.Auth;
using talk2note.Infrastructure.Data;
using talk2note.Infrastructure.External.Elastic;
using talk2note.Infrastructure.Persistence;
using talk2note.Infrastructure.Services;

namespace talk2note.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
        {
            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("Server=localhost;Database=talk2note;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;",
                    b => b.MigrationsAssembly("talk2note.API"));
            });
            var settings = new ConnectionSettings(new Uri("https://localhost:9200"))
                .DefaultIndex("notes")
                .BasicAuthentication("elastic", "vpDnDtUlbV2v7BW98-Kl")
                .ServerCertificateValidationCallback(CertificateValidations.AllowAll);  
            var client = new ElasticClient(settings);


       
            services.AddSingleton<IElasticClient>(client); 

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IElasticsearchService, ElasticsearchService>();
            services.AddScoped<INoteSearch, NoteSearch>();

    
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }

}
