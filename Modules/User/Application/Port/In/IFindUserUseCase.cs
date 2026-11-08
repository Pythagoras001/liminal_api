using liminal_api.Modules.User.Domain;

namespace liminal_api.Modules.User.Application.Port.In
{
    public interface IFindUserUseCase
    {
        Task<UserRegister> findUser(string id);
    }
}
