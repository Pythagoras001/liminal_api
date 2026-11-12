using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;

namespace liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Entities
{
    internal class LevelClassDbEntity
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? ClassNumber { get; set; }
        public string SecurityLevel { get; set; } = string.Empty;
        public string Legitimacy { get; set; } = string.Empty;
        public string DangerLevel { get; set; } = string.Empty;
        public string IconImageId { get; set; } = string.Empty;
        public ImageDbEntity IconImage { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
    }
}
