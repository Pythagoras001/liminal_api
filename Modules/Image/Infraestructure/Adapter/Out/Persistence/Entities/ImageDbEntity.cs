namespace liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities
{
    internal class ImageDbEntity
    {
        public string? Id { get; set; }
        public string? PublicId { get; set; }
        public string? OriginalUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? MediumUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public ImageDbEntity(string? id, string? publicId, string? originalUrl, string? thumbnailUrl, string? mediumUrl, DateTime createdAt)
        {
            Id = id;
            PublicId = publicId;
            OriginalUrl = originalUrl;
            ThumbnailUrl = thumbnailUrl;
            MediumUrl = mediumUrl;
            CreatedAt = createdAt;
        }

    }
}
