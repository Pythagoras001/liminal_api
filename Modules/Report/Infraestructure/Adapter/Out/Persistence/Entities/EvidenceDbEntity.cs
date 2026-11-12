using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Entities
{
    internal class EvidenceDbEntity
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageId { get; set; } = string.Empty;
        public ImageDbEntity Image { get; set; } = null!;
        public int? GaleryReportId { get; set; }
        public ReportDbEntity? GaleryReport { get; set; }
    }
}
