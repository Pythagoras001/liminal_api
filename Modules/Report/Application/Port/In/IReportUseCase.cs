using liminal_api.Modules.Report.Application.Command;
using ReportEntity = liminal_api.Modules.Report.Domain.Report;

namespace liminal_api.Modules.Report.Application.Port.In
{
    public interface IReportUseCase
    {
        Task<ReportEntity> Create(CreateReportCommand command);
        Task<ReportEntity> Rate(int reportId, string userId, bool liked);
        Task<ReportEntity> Update(UpdateReportCommand command);
        Task Delete(int reportId, string userId);
    }
}
