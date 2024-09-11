using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talk2note.Application.DTO.User;
using talk2note.Application.Services.Auth0;

namespace talk2note.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(UserSignUp registerDto);
        Task <string> LoginUserAsync(UserSignIn loginDto);
    }
}
