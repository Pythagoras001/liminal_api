using liminal_api.Modules.Image.Application.Command;
using liminal_api.Modules.Image.Application.Port.In;
using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Domain;

namespace liminal_api.Modules.Image.Application.Port.Service
{
    public class ImageService : ISaveImageUseCase
    {
        private readonly IImageRepositoryPort _imageRepository;
        private readonly ICloudinaryImageRepositoryPort _cloudinaryImageRepositoryPort;

        public ImageService(IImageRepositoryPort imageRepository, ICloudinaryImageRepositoryPort cloudinaryImageRepositoryPort)
        {
            _imageRepository = imageRepository;
            _cloudinaryImageRepositoryPort = cloudinaryImageRepositoryPort;
        }

        public async Task<ImageCloudinary> save(SaveImageCommand command)
        {
            ImageCloudinary imageCloudinary = await _cloudinaryImageRepositoryPort.UploadAsync(command.file, command.fileName);
            return await _imageRepository.save(imageCloudinary);
        }
    }
}
