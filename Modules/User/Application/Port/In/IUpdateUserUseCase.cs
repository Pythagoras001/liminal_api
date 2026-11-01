using liminal_api.Modules.User.Application.Command;
using liminal_api.Modules.User.Domain;

namespace liminal_api.Modules.User.Application.Port.In
{
    public interface IUpdateUserUseCase
    {
        Task<UserRegister> updateUserUseCase(UpdateUserCommand command);
    }
}
