using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talk2note.Application.DTO.User;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.UserService
{
    public interface IUserService
    {
        public  Task<bool> ChangePasswordAsync(int userId, ChangePassword changePasswordDto);
        Task<User> GetUserByCredentialsAsync(string email, string password);


    }
}
