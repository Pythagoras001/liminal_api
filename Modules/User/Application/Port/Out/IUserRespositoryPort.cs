using liminal_api.Modules.User.Domain;

namespace liminal_api.Modules.User.Application.Port.Out
{
    public interface IUserRespositoryPort
    {
        Task<UserRegister> save(UserRegister userRegister);

        Task<UserRegister> findById(long userId);

        Task<UserRegister?> findByEmail(string email);

        Task<UserRegister> update(UserRegister userRegister);

    }
}
