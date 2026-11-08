using System.ComponentModel.DataAnnotations;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest.Dto
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(20)]
        public string userName { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string email { get; set; } = default!;

        [Required]
        public string password { get; set; } = default!;
    }
}