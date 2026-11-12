using System.ComponentModel.DataAnnotations;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest.Dto
{
    public class FindAllReportsQueryDto
    {
        [Range(1, int.MaxValue)]
        public int? Page { get; set; }
    }
}
