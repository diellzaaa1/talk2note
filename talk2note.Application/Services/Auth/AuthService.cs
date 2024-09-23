using talk2note.Application.DTO.User;
using talk2note.Application.Interfaces;
using talk2note.Application.Services.UserService;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthTokenService _jwtTokenService;
        private readonly IUserService _userService;

        public AuthService(IUnitOfWork unitOfWork, AuthTokenService jwtTokenService, IUserService userService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task RegisterUserAsync(UserSignUp registerDto)
        {
            if (registerDto == null) throw new ArgumentNullException(nameof(registerDto));

            var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = HashPassword(registerDto.PasswordHash),
                CreatedAt = registerDto.CreatedAt
            };

            await _unitOfWork.Users.AddAsync(user); 
            await _unitOfWork.CommitAsync(); 
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
