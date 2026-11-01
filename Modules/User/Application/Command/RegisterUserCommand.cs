namespace liminal_api.Modules.User.Application.Command
{
    public record RegisterUserCommand(string userName, string email, string password);

}
