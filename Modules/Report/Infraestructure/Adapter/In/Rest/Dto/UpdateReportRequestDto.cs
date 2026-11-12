using System.ComponentModel.DataAnnotations;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest.Dto
{
    public class UpdateReportRequestDto
    {
        [MinLength(1)]
        public string? Title { get; set; }

        [MinLength(1)]
        public string? Nivel { get; set; }

        [Range(1, int.MaxValue)]
        public int? LevelClassId { get; set; }

        [MinLength(1)]
        public string? Description { get; set; }
    }
}
