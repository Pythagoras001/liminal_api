using liminal_api.Modules.User.Application.Command;
using liminal_api.Modules.User.Application.Port.In;
using liminal_api.Modules.User.Infraestructure.Adapter.In.Rest.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILoginUserUseCase _loginUserUseCase;
        private readonly IRegisterUserUseCase _registerUserUseCase;

        public UserController(ILoginUserUseCase loginUserUseCase, IRegisterUserUseCase registerUserUseCase)
        {
            _loginUserUseCase = loginUserUseCase;
            _registerUserUseCase = registerUserUseCase;
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
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest(new { message = "Invalid user data." });
            }
        }
    }
}
