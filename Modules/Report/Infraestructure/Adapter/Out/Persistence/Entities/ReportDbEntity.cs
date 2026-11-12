using liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Entities;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Entities
{
    internal class ReportDbEntity
    {
        public int Id { get; set; }
        public long AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public int LevelClassId { get; set; }
        public LevelClassDbEntity LevelClass { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string PrincipalEvidenceId { get; set; } = string.Empty;
        public EvidenceDbEntity PrincipalEvidence { get; set; } = null!;
        public List<EvidenceDbEntity> GaleryEvidences { get; set; } = [];
        public List<ReportLikeDbEntity> Likes { get; set; } = [];
    }
}
