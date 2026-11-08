namespace liminal_api.Modules.User.Application.Command
{
    public record UpdateUserCommand(long userId, string userName, string email, Stream imgProfile);
  
}
