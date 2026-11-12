using System.ComponentModel.DataAnnotations;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest.Dto
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression(
            @"^[a-zA-Z0-9_]+$",
            ErrorMessage = "userName only accepts letters, numbers and underscores")]
        public string userName { get; set; } = default!;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string email { get; set; } = default!;

        [Required]
        [MinLength(8)]
        [MaxLength(72)]
        public string password { get; set; } = default!;
    }
}