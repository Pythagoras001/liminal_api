using System.ComponentModel.DataAnnotations;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest.Dto
{
    public class RateReportRequestDto
    {
        [Range(1, int.MaxValue)]
        public int ReportId { get; set; }

        [Required]
        public bool? Liked { get; set; }
    }
}
