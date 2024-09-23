using Microsoft.Extensions.DependencyInjection;
using talk2note.Application.Interfaces;
using talk2note.Application.Services.Auth;
using talk2note.Application.Services.NoteService;
using talk2note.Application.Services.NoteToDoService;
using talk2note.Application.Services.UserService;

namespace talk2note.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<INoteToDoService, NoteToDoService>();

            return services;
        }
    }
}
