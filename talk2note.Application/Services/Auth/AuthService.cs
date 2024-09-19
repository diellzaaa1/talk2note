using talk2note.Application.DTO.User;
using talk2note.Application.Interfaces;
using talk2note.Application.Services.UserService;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthTokenService _jwtTokenService;
        private readonly IUserService _userService;

        public AuthService(IUserRepository userRepository, AuthTokenService jwtTokenService,IUserService userService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _userService= userService ?? throw new ArgumentNullException( nameof(userService));
        }

        public async Task RegisterUserAsync(UserSignUp registerDto)
        {
            if (registerDto == null) throw new ArgumentNullException(nameof(registerDto));

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

        public async Task<string> LoginAsync(UserSignIn dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var user = await _userService.GetUserByCredentialsAsync(dto.Email, dto.Password);
            if (user != null)
            {   
                    return _jwtTokenService.GenerateToken(user);  
            }
            throw new UnauthorizedAccessException("User not found.");
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
