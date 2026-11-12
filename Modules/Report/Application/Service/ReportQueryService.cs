using liminal_api.Modules.LevelClass.Application.Port.In;
using liminal_api.Modules.Report.Application.Port.Out;
using liminal_api.Modules.Report.Application.ReadModel;
using liminal_api.Modules.User.Application.Service;
using ReportEntity = liminal_api.Modules.Report.Domain.Report;

namespace liminal_api.Modules.Report.Application.Service
{
    public class ReportQueryService
    {
        private const int ReportsPageSize = 9;
        private readonly IReportRepositoryPort _repository;
        private readonly UserService _userService;
        private readonly ILevelClassUseCase _levelClassUseCase;

        public ReportQueryService(
            IReportRepositoryPort repository,
            UserService userService,
            ILevelClassUseCase levelClassUseCase)
        {
            _repository = repository;
            _userService = userService;
            _levelClassUseCase = levelClassUseCase;
        }

        public Task<PaginatedResult<ReportDetailReadModel>> FindAll(int page = 1) =>
            FindPage(null, page);

        public Task<PaginatedResult<ReportDetailReadModel>> FindByAuthor(
            string authorId, int page = 1) => FindPage(authorId, page);

        public async Task<ReportDetailReadModel> FindById(int id)
        {
            ReportEntity report = await _repository.FindById(id)
                ?? throw new KeyNotFoundException($"Report with id {id} not found.");
            return await ToReadModel(report);
        }

        private async Task<PaginatedResult<ReportDetailReadModel>> FindPage(
            string? authorId, int page)
        {
            int currentPage = Math.Max(page, 1);
            int skip = (currentPage - 1) * ReportsPageSize;
            Task<IReadOnlyList<ReportEntity>> reportsTask = authorId == null
                ? _repository.FindAll(skip, ReportsPageSize)
                : _repository.FindByAuthor(authorId, skip, ReportsPageSize);
            Task<int> countTask = authorId == null
                ? _repository.Count()
                : _repository.CountByAuthor(authorId);

            await Task.WhenAll(reportsTask, countTask);
            var data = await Task.WhenAll(reportsTask.Result.Select(ToReadModel));
            int total = countTask.Result;

            return new PaginatedResult<ReportDetailReadModel>(
                data,
                currentPage,
                ReportsPageSize,
                total,
                (int)Math.Ceiling(total / (double)ReportsPageSize));
        }

        private async Task<ReportDetailReadModel> ToReadModel(ReportEntity report)
        {
            var authorTask = _userService.findUser(report.AuthorId);
            var levelClassTask = _levelClassUseCase.FindById(report.LevelClassId);
            await Task.WhenAll(authorTask, levelClassTask);
            return new ReportDetailReadModel(
                report,
                authorTask.Result,
                levelClassTask.Result);
        }
    }
}
