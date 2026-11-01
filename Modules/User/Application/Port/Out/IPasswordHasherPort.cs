namespace liminal_api.Modules.User.Application.Port.Out
{
    public interface IPasswordHasherPort
    {
        string hashPassword(string password);
        bool verifyPassword(string hashedPassword, string providedPassword);
    }
}
