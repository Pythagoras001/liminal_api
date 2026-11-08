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

        public async Task<ImageCloudinary> changeImageAsync(Stream file, string fileName, string? publicId)
        {
            if (publicId != null)
            {
                var deleteResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
                if (deleteResult.Error != null)
                    throw new InvalidOperationException($"Cloudinary delete failed: {deleteResult.Error.Message}");
            }

            return await UploadAsync(file, fileName);
        }

        public async Task deleteImage(string publicId)
        {
            var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            if (result.Result == "not found")
                throw new KeyNotFoundException($"Image with public id '{publicId}' not found in Cloudinary.");

            if (result.Error != null)
                throw new InvalidOperationException($"Cloudinary delete failed: {result.Error.Message}");
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