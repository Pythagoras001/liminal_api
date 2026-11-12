using liminal_api.Modules.LevelClass.Domain.Enums;

namespace liminal_api.Modules.LevelClass.Application.Command
{
    public record UpdateLevelClassCommand(
        int Id,
        SurvivalClassType? Type,
        string? ClassNumber,
        string? SecurityLevel,
        Legitimacy? Legitimacy,
        string? DangerLevel,
        string? Description,
        Stream? Icon);
}
