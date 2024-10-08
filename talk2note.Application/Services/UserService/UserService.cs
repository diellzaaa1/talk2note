﻿using talk2note.Application.DTO.User;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
         
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
            var user = await _unitOfWork.Users.GetByEmailAsync(email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task<User> GetOrCreateUserAsync(string email, string name)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                user = new User { Email = email, Name = name };
                await _unitOfWork.Users.AddAsync(user);
            }
            await _unitOfWork.CommitAsync();

            return user;
        }

    }

}

