using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Domain;

namespace liminal_api.Modules.Image.Infraestructure.Adapter.Out.Cloudinary
{
    internal class CloudinaryImageStorageAdapter : ICloudinaryImageRepositoryPort
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public CloudinaryImageStorageAdapter(CloudinaryDotNet.Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<ImageCloudinary> UploadAsync(Stream file, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, file)
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");

            return new ImageCloudinary(
                publicId: result.PublicId,
                originalUrl: result.SecureUrl.ToString(),
                thumbnailUrl: BuildUrl(result.PublicId, new Transformation().Width(150).Height(150).Crop("fill")),
                mediumUrl: BuildUrl(result.PublicId, new Transformation().Width(600).Crop("scale"))
            );
        }

        // = buildImageVariantUrls
        private string BuildUrl(string publicId, Transformation transformation) =>
            _cloudinary.Api.UrlImgUp
                .Secure(true)
                .Transform(transformation)
                .BuildUrl(publicId);
    }
}