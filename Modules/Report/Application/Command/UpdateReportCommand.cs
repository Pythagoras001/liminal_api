namespace liminal_api.Modules.Report.Application.Command
{
    public record UpdateReportCommand(
        int Id,
        string UserId,
        string? Title,
        string? Nivel,
        int? LevelClassId,
        string? Description);
}
