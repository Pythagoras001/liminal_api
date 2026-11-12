namespace liminal_api.Modules.Report.Domain
{
    public class ReportLike
    {
        public string UserId { get; }
        public int? ReportId { get; }
        public DateTime CreatedAt { get; }

        public ReportLike(string userId, int? reportId, DateTime createdAt)
        {
            UserId = userId;
            ReportId = reportId;
            CreatedAt = createdAt;
        }
    }
}
