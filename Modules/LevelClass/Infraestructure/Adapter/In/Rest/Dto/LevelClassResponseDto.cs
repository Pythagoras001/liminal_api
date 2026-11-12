using liminal_api.Modules.LevelClass.Domain.Enums;
using LevelClassEntity = liminal_api.Modules.LevelClass.Domain.LevelClass;

namespace liminal_api.Modules.LevelClass.Infraestructure.Adapter.In.Rest.Dto
{
    public class LevelClassResponseDto
    {
        public int Id { get; init; }
        public SurvivalClassType Type { get; init; }
        public string? ClassNumber { get; init; }
        public string SecurityLevel { get; init; } = string.Empty;
        public Legitimacy Legitimacy { get; init; }
        public string DangerLevel { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public IconImageResponseDto IconImage { get; init; } = new();

        public static LevelClassResponseDto FromDomain(LevelClassEntity levelClass) => new()
        {
            Id = levelClass.Id!.Value,
            Type = levelClass.Type,
            ClassNumber = levelClass.ClassNumber,
            SecurityLevel = levelClass.SecurityLevel,
            Legitimacy = levelClass.Legitimacy,
            DangerLevel = levelClass.DangerLevel,
            Description = levelClass.Description,
            IconImage = new IconImageResponseDto
            {
                ThumbnailUrl = levelClass.IconImage.ThumbnailUrl ?? string.Empty,
                MediumUrl = levelClass.IconImage.MediumUrl ?? string.Empty,
                OriginalUrl = levelClass.IconImage.OriginalUrl ?? string.Empty
            }
        };

        public class IconImageResponseDto
        {
            public string ThumbnailUrl { get; init; } = string.Empty;
            public string MediumUrl { get; init; } = string.Empty;
            public string OriginalUrl { get; init; } = string.Empty;
        }
    }
}
