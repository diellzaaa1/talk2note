using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talk2note.Application.DTO.User;
using talk2note.Domain.Entities;

namespace talk2note.Application.Interfaces
{
    public interface IAuthService
    {

        Task<string> LoginAsync(UserSignIn dto);
        Task RegisterUserAsync(UserSignUp registerDto);

  
    }
}
