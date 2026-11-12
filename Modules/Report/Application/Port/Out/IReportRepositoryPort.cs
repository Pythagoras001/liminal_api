using ReportEntity = liminal_api.Modules.Report.Domain.Report;

namespace liminal_api.Modules.Report.Application.Port.Out
{
    public interface IReportRepositoryPort
    {
        Task<ReportEntity> Create(ReportEntity report);
        Task<IReadOnlyList<ReportEntity>> FindAll(int skip, int take);
        Task<IReadOnlyList<ReportEntity>> FindByAuthor(string authorId, int skip, int take);
        Task<ReportEntity?> FindById(int id);
        Task<int> Count();
        Task<int> CountByAuthor(string authorId);
        Task<ReportEntity> Update(ReportEntity report);
        Task Delete(int id);
    }
}
