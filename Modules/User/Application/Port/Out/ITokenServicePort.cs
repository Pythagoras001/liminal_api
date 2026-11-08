using liminal_api.Modules.User.Domain;

namespace liminal_api.Modules.User.Application.Port.Out
{
    public interface ITokenServicePort
    {
        string generateToken(UserRegister user);
    }
}