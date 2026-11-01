namespace liminal_api.Modules.Image.Application.Command
{
    public record SaveImageCommand(Stream file, string fileName);
}
