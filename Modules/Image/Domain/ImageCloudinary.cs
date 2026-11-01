namespace liminal_api.Modules.Image.Domain
{
    public class ImageCloudinary
    {
        private readonly string _id;
        private readonly string? _publicId;
        private readonly string? _originalUrl;
        private readonly string? _thumbnailUrl;
        private readonly string? _mediumUrl;
        private DateTime _createdAt;

        public ImageCloudinary(string? publicId, string? originalUrl, string? thumbnailUrl, string? mediumUrl)
        {
            _publicId = publicId;
            _originalUrl = originalUrl;
            _thumbnailUrl = thumbnailUrl;
            _mediumUrl = mediumUrl;
        }

        public ImageCloudinary(string? id, string? publicId, string? originalUrl, string? thumbnailUrl, string? mediumUrl, DateTime createdAt)
        {
            _id = id;
            _publicId = publicId;
            _originalUrl = originalUrl;
            _thumbnailUrl = thumbnailUrl;
            _mediumUrl = mediumUrl;
            _createdAt = createdAt;
        }

        public string? Id => _id;

        public string? PublicId => _publicId;

        public string? OriginalUrl => _originalUrl;

        public string? ThumbnailUrl => _thumbnailUrl;

        public string? MediumUrl => _mediumUrl;

        public DateTime CreatedAt => _createdAt;

        public ImageCloudinary initDefaults()
        {
            this._createdAt = DateTime.UtcNow;
            return this;
        }

    }
}
