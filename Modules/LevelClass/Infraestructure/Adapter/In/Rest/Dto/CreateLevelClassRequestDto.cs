using System.ComponentModel.DataAnnotations;
using liminal_api.Modules.LevelClass.Domain.Enums;

namespace liminal_api.Modules.LevelClass.Infraestructure.Adapter.In.Rest.Dto
{
    public class CreateLevelClassRequestDto
    {
        [Required]
        [EnumDataType(typeof(SurvivalClassType))]
        public SurvivalClassType? Type { get; set; }

        public string? ClassNumber { get; set; }

        [Required]
        public string SecurityLevel { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(Legitimacy))]
        public Legitimacy? Legitimacy { get; set; }

        [Required]
        public string DangerLevel { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public IFormFile? Icon { get; set; }
    }
}
