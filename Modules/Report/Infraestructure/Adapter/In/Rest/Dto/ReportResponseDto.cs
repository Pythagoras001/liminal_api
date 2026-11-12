using liminal_api.Modules.Report.Domain;
using ReportEntity = liminal_api.Modules.Report.Domain.Report;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest.Dto
{
    public class ReportResponseDto
    {
        public int Id { get; init; }
        public string AuthorId { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Nivel { get; init; } = string.Empty;
        public int LevelClassId { get; init; }
        public string Description { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public EvidenceResponseDto PrincipalEvidence { get; init; } = new();
        public IReadOnlyList<EvidenceResponseDto> GaleryEvidences { get; init; } = [];
        public int LikesCount { get; init; }

        public static ReportResponseDto FromDomain(ReportEntity report) => new()
        {
            Id = report.Id!.Value,
            AuthorId = report.AuthorId,
            Title = report.Title,
            Nivel = report.Nivel,
            LevelClassId = report.LevelClassId,
            Description = report.Description,
            CreatedAt = report.CreatedAt,
            PrincipalEvidence = EvidenceResponseDto.FromDomain(report.PrincipalEvidence),
            GaleryEvidences = report.GaleryEvidences
                .Select(EvidenceResponseDto.FromDomain)
                .ToList(),
            LikesCount = report.LikesCount
        };
    }

    public class EvidenceResponseDto
    {
        public string Description { get; init; } = string.Empty;
        public EvidenceImageResponseDto Image { get; init; } = new();

        internal static EvidenceResponseDto FromDomain(Evidence evidence) => new()
        {
            Description = evidence.Description,
            Image = new EvidenceImageResponseDto
            {
                ThumbnailUrl = evidence.Image.ThumbnailUrl ?? string.Empty,
                MediumUrl = evidence.Image.MediumUrl ?? string.Empty,
                OriginalUrl = evidence.Image.OriginalUrl ?? string.Empty
            }
        };
    }

    public class EvidenceImageResponseDto
    {
        public string ThumbnailUrl { get; init; } = string.Empty;
        public string MediumUrl { get; init; } = string.Empty;
        public string OriginalUrl { get; init; } = string.Empty;
    }
}
