using System.ComponentModel.DataAnnotations;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest.Dto
{
    public class LoginRequestDto
    {
        [Required]
        public string password { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string email { get; set; } = default!;
    }
}



