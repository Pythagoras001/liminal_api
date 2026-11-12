using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest.Dto
{
    public class UpdateUserRequestDto
    {
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression(
            @"^[a-zA-Z0-9_]+$",
            ErrorMessage = "userName only accepts letters, numbers and underscores")]
        public string? userName { get; set; }

        [MinLength(1)]
        [EmailAddress]
        [MaxLength(255)]
        public string? email { get; set; }

        public IFormFile? avatar { get; set; }
        public IFormFile? imgProfile { get; set; }
    }
}
