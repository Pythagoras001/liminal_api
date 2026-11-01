namespace liminal_api.Modules.User.Application.Command
{
    public record UpdateUserCommand(string userId, string userName, string email);
  
}
