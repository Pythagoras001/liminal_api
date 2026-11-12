using System.ComponentModel.DataAnnotations;
using liminal_api.Modules.LevelClass.Domain.Enums;

namespace liminal_api.Modules.LevelClass.Infraestructure.Adapter.In.Rest.Dto
{
    public class UpdateLevelClassRequestDto
    {
        [EnumDataType(typeof(SurvivalClassType))]
        public SurvivalClassType? Type { get; set; }

        public string? ClassNumber { get; set; }
        public string? SecurityLevel { get; set; }

        [EnumDataType(typeof(Legitimacy))]
        public Legitimacy? Legitimacy { get; set; }

        public string? DangerLevel { get; set; }
        public string? Description { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
