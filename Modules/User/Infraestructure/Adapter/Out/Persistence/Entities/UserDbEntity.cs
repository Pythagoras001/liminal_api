using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;

namespace liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Entities
{
    internal class UserDbEntity
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ImgProfileId { get; set; }     
        public ImageDbEntity? ImgProfile { get; set; }
        public DateTime CreatedAt { get; set; }


        public UserDbEntity(string id, string? userName, string? email, string? password, string? imgProfileId, DateTime createdAt)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Password = password;
            ImgProfileId = imgProfileId;
            CreatedAt = createdAt;
        }
    }
}
