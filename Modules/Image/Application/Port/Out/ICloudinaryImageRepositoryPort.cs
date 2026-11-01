using liminal_api.Modules.Image.Domain;

namespace liminal_api.Modules.Image.Application.Port.Out
{
    public interface ICloudinaryImageRepositoryPort
    {
        Task<ImageCloudinary> UploadAsync(Stream file, string fileName);
    }

}
