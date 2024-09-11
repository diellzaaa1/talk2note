using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talk2note.Application.Interfaces;
using talk2note.Application.Services;
using talk2note.Application.Services.Auth0;

namespace talk2note.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuth0Service, Auth0Service>();


            return services;
        }
    }
}
