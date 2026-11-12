namespace liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Entities
{
    internal class ReportLikeDbEntity
    {
        public long UserId { get; set; }
        public int ReportId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
