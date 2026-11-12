using liminal_api.Modules.LevelClass.Domain;
using liminal_api.Modules.User.Domain;
using ReportEntity = liminal_api.Modules.Report.Domain.Report;
using LevelClassEntity = liminal_api.Modules.LevelClass.Domain.LevelClass;

namespace liminal_api.Modules.Report.Application.ReadModel
{
    public record ReportDetailReadModel(
        ReportEntity Report,
        UserRegister Author,
        LevelClassEntity LevelClass);
}
