using liminal_api.Modules.Image.Domain;

namespace liminal_api.Modules.Report.Domain
{
    public class Evidence
    {
        public string? Id { get; }
        public ImageCloudinary Image { get; }
        public string Description { get; private set; }

        public Evidence(string? id, ImageCloudinary image, string description)
        {
            Id = id;
            Image = image;
            Description = description;
        }

        public void ChangeDescription(string description) => Description = description;
    }
}
