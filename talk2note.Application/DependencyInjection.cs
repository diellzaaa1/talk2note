using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using talk2note.Application.Interfaces;
using talk2note.Application.Services.Auth;
using talk2note.Application.Services.EmailService;
using talk2note.Application.Services.NoteService;
using talk2note.Application.Services.NoteToDoService;
using talk2note.Application.Services.UserService;
using IMailService = talk2note.Application.Services.EmailService.IMailService;

namespace talk2note.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailService, MailService>();

            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<INoteToDoService, NoteToDoService>();

            return services;
        }
    }
}
