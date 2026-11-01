using liminal_api.Modules.Image.Application.Command;
using liminal_api.Modules.Image.Domain;

namespace liminal_api.Modules.Image.Application.Port.In
{
    public interface ISaveImageUseCase
    {
        public Task<ImageCloudinary> save(SaveImageCommand command);
    }
}
