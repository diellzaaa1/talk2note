using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using talk2note.Application.DTO.User;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;


        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePassword changePasswordDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash);

            if (!isCurrentPasswordValid)
            {
                return false; 
            }

           
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            user.PasswordHash = newPasswordHash;

            _unitOfWork.Users.Update(user);

            await _unitOfWork.CommitAsync();

            return true;
        }
        public async Task<User> GetUserByCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task<User> GetOrCreateUserAsync(string email, string name)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                user = new User { Email = email, Name = name };
                await _userRepository.AddAsync(user);
            }
            await _unitOfWork.CommitAsync();

            return user;
        }

    }

}

