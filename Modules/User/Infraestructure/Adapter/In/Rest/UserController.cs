using System.Security.Claims;
using liminal_api.Modules.User.Application.Command;
using liminal_api.Modules.User.Application.Port.In;
using liminal_api.Modules.User.Application.Service;
using liminal_api.Modules.User.Infraestructure.Adapter.In.Rest.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILoginUserUseCase _loginUserUseCase;
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly UserService _userService;

        public UserController(ILoginUserUseCase loginUserUseCase, IRegisterUserUseCase registerUserUseCase, UserService userService)
        {
            _loginUserUseCase = loginUserUseCase;
            _registerUserUseCase = registerUserUseCase;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequestDto request)
        {
            try
            {
                var command = new LoginCommand(request.email, request.password);
                var token = await _loginUserUseCase.loginUserUseCase(command);
                return Ok(new LoginResponseDto { token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var command = new RegisterUserCommand(request.userName, request.email, request.password);
                var userRegister = await _registerUserUseCase.registerUserUseCase(command);

                return Ok(new UserRegisterResponseDto
                {
                    id = userRegister.Id.ToString(),
                    userName = userRegister.UserName ?? string.Empty,
                    email = userRegister.Email ?? string.Empty,
                    createdAt = userRegister.CreatedAt,
                    profileImage = userRegister.ImgProfile == null ? null : new UserRegisterResponseDto.ProfileImageResponseDto
                    {
                        thumbnailUrl = userRegister.ImgProfile.ThumbnailUrl ?? string.Empty,
                        mediumUrl = userRegister.ImgProfile.MediumUrl ?? string.Empty,
                        originalUrl = userRegister.ImgProfile.OriginalUrl ?? string.Empty
                    }
                });
            }
            catch (ArgumentException)
            {
                return BadRequest(new { message = "Invalid user data." });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> getCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            try
            {
                var userRegister = await _userService.findUser(userIdClaim);

                return Ok(new UserRegisterResponseDto
                {
                    id = userRegister.Id.ToString(),
                    userName = userRegister.UserName ?? string.Empty,
                    email = userRegister.Email ?? string.Empty,
                    createdAt = userRegister.CreatedAt,
                    profileImage = userRegister.ImgProfile == null ? null : new UserRegisterResponseDto.ProfileImageResponseDto
                    {
                        thumbnailUrl = userRegister.ImgProfile.ThumbnailUrl ?? string.Empty,
                        mediumUrl = userRegister.ImgProfile.MediumUrl ?? string.Empty,
                        originalUrl = userRegister.ImgProfile.OriginalUrl ?? string.Empty
                    }
                });
            }
            catch (ArgumentException)
            {
                return BadRequest(new { message = "Invalid user id." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found." });
            }
        }
    }
}
