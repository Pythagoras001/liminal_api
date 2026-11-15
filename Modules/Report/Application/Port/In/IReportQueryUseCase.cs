using liminal_api.Modules.Report.Application.ReadModel;

namespace liminal_api.Modules.Report.Application.Port.In
{
    public interface IReportQueryUseCase
    {
        Task<PaginatedResult<ReportDetailReadModel>> FindAll(int page = 1);
        Task<PaginatedResult<ReportDetailReadModel>> FindByAuthor(string authorId, int page = 1);
        Task<ReportDetailReadModel> FindById(int id);
    }
}
