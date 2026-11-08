using Microsoft.AspNetCore.Http;

namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest.Dto
{
    public class UpdateUserRequestDto
    {
        public string? userName { get; set; }
        public string? email { get; set; }
        public IFormFile? imgProfile { get; set; }
    }
}
