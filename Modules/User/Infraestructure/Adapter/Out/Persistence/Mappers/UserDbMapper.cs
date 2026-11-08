using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Mappers;
using liminal_api.Modules.User.Domain;
using liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Entities;
using Riok.Mapperly.Abstractions;

namespace liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Mappers
{
    [Mapper]
    [UseStaticMapper(typeof(ImageDbMapper))]
    internal static partial class UserDbMapper
    {
        // ImgProfileId se obtiene de forma segura cuando el usuario no tiene imagen.
        [MapProperty("ImgProfile.Id", nameof(UserDbEntity.ImgProfileId))]
        [MapperIgnoreTarget(nameof(UserDbEntity.ImgProfile))]
        internal static partial UserDbEntity ToEntity(this UserRegister userRegister);

        // La imagen es opcional, por lo que no se debe lanzar una excepción si falta.
        [MapperIgnoreSource(nameof(UserDbEntity.ImgProfileId))]
        internal static partial UserRegister ToDomain(this UserDbEntity userDbEntity);

    }
}
