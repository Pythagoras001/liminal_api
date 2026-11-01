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
        // ImgProfileId se llena solo desde ImgProfile.Id (Mapperly lo hace automático).
        // La navegación se ignora para que EF no intente insertar la imagen otra vez.
        [MapperIgnoreTarget(nameof(UserDbEntity.ImgProfile))]
        internal static partial UserDbEntity ToEntity(this UserRegister userRegister);

        [MapperIgnoreSource(nameof(UserDbEntity.ImgProfileId))]
        internal static partial UserRegister ToDomain(this UserDbEntity userDbEntity);

    }
}
