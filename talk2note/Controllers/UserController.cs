using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using talk2note.Application.DTO.User;
using talk2note.Application.Interfaces;
using talk2note.Application.Services.Auth;
using talk2note.Application.Services.UserService;
using talk2note.Domain.Entities;

namespace talk2note.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly AuthTokenService _authTokenService;

        public UserController(IAuthService authService, IUnitOfWork unitOfWork, IUserService userService, AuthTokenService authTokenService)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _authTokenService = authTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserSignUp registerDto)
        {
            try
            {
                await _authService.RegisterUserAsync(registerDto);
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserSignIn dto)
        {
            var token = await _authService.LoginAsync(dto);
            return token != null ? Ok(new { Token = token }) : Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.ChangePasswordAsync(changePasswordDto.Id, changePasswordDto);
                return result ? Ok("Password changed successfully.") : BadRequest("Current password is incorrect.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "User", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (info?.Principal == null)
                return Unauthorized();

            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            var userName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? "Default Name"; // Provide a fallback name

            var user = await _userService.GetOrCreateUserAsync(userEmail, userName);
            var token = _authTokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }

    }
}
