using System;
using System.Threading.Tasks;
using talk2note.Application.DTO.User;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using BCrypt.Net;

namespace talk2note.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(UserSignUp registerDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Username = registerDto.Username,
                PasswordHash = HashPassword(registerDto.PasswordHash),
                CreatedAt = registerDto.CreatedAt
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<string> LoginUserAsync(UserSignIn loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(user.PasswordHash, loginDto.Password))
            {
                throw new Exception("Invalid credentials");
            }

            return TokenService.GenerateToken(loginDto.Email);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string hashedPassword, string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
