using liminal_api.Modules.Report.Application.ReadModel;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest.Dto
{
    public class ReportDetailResponseDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Nivel { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public int LikesCount { get; init; }
        public EvidenceResponseDto PrincipalEvidence { get; init; } = new();
        public IReadOnlyList<EvidenceResponseDto> GaleryEvidences { get; init; } = [];
        public AuthorResponseDto Author { get; init; } = new();
        public LevelClassResponseDto LevelClass { get; init; } = new();

        public static ReportDetailResponseDto FromReadModel(ReportDetailReadModel model)
        {
            var report = model.Report;
            var profileImage = model.Author.ImgProfile;
            var levelClass = model.LevelClass;

            return new ReportDetailResponseDto
            {
                Id = report.Id!.Value,
                Title = report.Title,
                Nivel = report.Nivel,
                Description = report.Description,
                CreatedAt = report.CreatedAt,
                LikesCount = report.LikesCount,
                PrincipalEvidence = EvidenceResponseDto.FromDomain(report.PrincipalEvidence),
                GaleryEvidences = report.GaleryEvidences
                    .Select(EvidenceResponseDto.FromDomain)
                    .ToList(),
                Author = new AuthorResponseDto
                {
                    Id = model.Author.Id.ToString(),
                    UserName = model.Author.UserName ?? string.Empty,
                    ProfileImage = profileImage == null ? null : new EvidenceImageResponseDto
                    {
                        ThumbnailUrl = profileImage.ThumbnailUrl ?? string.Empty,
                        MediumUrl = profileImage.MediumUrl ?? string.Empty,
                        OriginalUrl = profileImage.OriginalUrl ?? string.Empty
                    }
                },
                LevelClass = new LevelClassResponseDto
                {
                    Id = levelClass.Id!.Value,
                    Type = levelClass.Type.ToString(),
                    ClassNumber = levelClass.ClassNumber,
                    SecurityLevel = levelClass.SecurityLevel,
                    Legitimacy = levelClass.Legitimacy.ToString(),
                    DangerLevel = levelClass.DangerLevel,
                    Description = levelClass.Description,
                    IconImage = new EvidenceImageResponseDto
                    {
                        ThumbnailUrl = levelClass.IconImage.ThumbnailUrl ?? string.Empty,
                        MediumUrl = levelClass.IconImage.MediumUrl ?? string.Empty,
                        OriginalUrl = levelClass.IconImage.OriginalUrl ?? string.Empty
                    }
                }
            };
        }
    }

    public class AuthorResponseDto
    {
        public string Id { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public EvidenceImageResponseDto? ProfileImage { get; init; }
    }

    public class LevelClassResponseDto
    {
        public int Id { get; init; }
        public string Type { get; init; } = string.Empty;
        public string? ClassNumber { get; init; }
        public string SecurityLevel { get; init; } = string.Empty;
        public string Legitimacy { get; init; } = string.Empty;
        public string DangerLevel { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public EvidenceImageResponseDto IconImage { get; init; } = new();
    }
}
