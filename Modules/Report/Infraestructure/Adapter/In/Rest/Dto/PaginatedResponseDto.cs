namespace liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest.Dto
{
    public record PaginatedResponseDto<T>(
        IReadOnlyList<T> Data,
        int Page,
        int PageSize,
        int Total,
        int TotalPages);
}
