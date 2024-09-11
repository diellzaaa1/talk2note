using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using talk2note.Application.DTO.User;
using talk2note.Application.Interfaces;
using talk2note.Infrastructure.Persistence;

namespace talk2note.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
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
        public async Task<IActionResult> Login(UserSignIn loginDto)
        {
                var token = await _authService.LoginUserAsync(loginDto);
            return Ok(token);
           
        
    }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
