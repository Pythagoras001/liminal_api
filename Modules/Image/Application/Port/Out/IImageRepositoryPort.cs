
namespace liminal_api.Modules.Image.Application.Port.Out
{
    using liminal_api.Modules.Image.Domain;
    public interface IImageRepositoryPort
    {
        Task<ImageCloudinary> save(ImageCloudinary image);
    }
}
