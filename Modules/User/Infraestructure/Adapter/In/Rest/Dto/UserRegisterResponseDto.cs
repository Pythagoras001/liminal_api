namespace liminal_api.Modules.User.Infraestructure.Adapter.In.Rest.Dto
{
    public class UserRegisterResponseDto
    {
        public string id { get; set; } = default!;
        public string userName { get; set; } = default!;
        public string email { get; set; } = default!;
        public DateTime createdAt { get; set; }
        public ProfileImageResponseDto? profileImage { get; set; }

        public class ProfileImageResponseDto
        {
            public string thumbnailUrl { get; set; } = default!;
            public string mediumUrl { get; set; } = default!;
            public string originalUrl { get; set; } = default!;
        }
    }
}
