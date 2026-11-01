using liminal_api.Modules.User.Application.Command;

namespace liminal_api.Modules.User.Application.Port.In
{
    public interface IRegisterUserUseCase
    {
        Task<Domain.UserRegister> registerUserUseCase(RegisterUserCommand command);
    }
}
