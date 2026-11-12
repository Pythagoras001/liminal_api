using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest.Dto
{
    public class CreateReportRequestDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Nivel { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int LevelClassId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string PrincipalEvidenceDescription { get; set; } = string.Empty;

        [FromForm(Name = "galeryEvidencesDescriptions")]
        public string[]? GaleryEvidencesDescriptions { get; set; }

        [Required]
        public IFormFile? PrincipalEvidence { get; set; }

        [FromForm(Name = "galeryEvidences")]
        public List<IFormFile>? GaleryEvidences { get; set; }
    }
}
