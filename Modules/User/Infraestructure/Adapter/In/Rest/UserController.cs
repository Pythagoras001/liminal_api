using liminal_api.Modules.User.Application.Command;
using liminal_api.Modules.User.Application.Port.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILoginUserUseCase _loginUserUseCase;

        public UserController(ILoginUserUseCase loginUserUseCase)
        {
            _loginUserUseCase = loginUserUseCase;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginCommand command)
        {
            try
            {
                var user = await _loginUserUseCase.loginUserUseCase(command);
                return Ok(new
                {
                    token = user.Token,
                    user = new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        email = user.Email
                    }
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
        }
    }
}
