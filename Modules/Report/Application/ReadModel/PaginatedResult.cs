namespace liminal_api.Modules.Report.Application.ReadModel
{
    public record PaginatedResult<T>(
        IReadOnlyList<T> Data,
        int Page,
        int PageSize,
        int Total,
        int TotalPages);
}
