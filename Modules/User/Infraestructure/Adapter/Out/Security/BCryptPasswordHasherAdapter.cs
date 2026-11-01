using liminal_api.Modules.User.Application.Port.Out;

namespace liminal_api.Modules.User.Infraestructure.Adapter.Out.Security
{
    internal class BCryptPasswordHasherAdapter : IPasswordHasherPort
    {
        private const int SaltRounds = 10;  

        public string hashPassword(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password, SaltRounds);

        public bool verifyPassword(string hashedPassword, string providedPassword) =>
            BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}
