using liminal_api.Modules.User.Application.Command;
using liminal_api.Modules.User.Domain;

namespace liminal_api.Modules.User.Application.Port.In
{
    public interface ILoginUserUseCase
    {
        Task<UserRegister> loginUserUseCase(LoginCommand command);
    }
}