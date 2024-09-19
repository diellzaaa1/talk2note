using Microsoft.AspNetCore.Mvc;
using talk2note.Application.DTO.User;
using talk2note.Application.Interfaces;
using talk2note.Application.Services.UserService;


namespace talk2note.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public UserController(IAuthService authService, IUnitOfWork unitOfWork,IUserService userService )
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _userService = userService;
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
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
       
     
    }
}
