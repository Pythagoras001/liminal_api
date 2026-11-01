
using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;
using Riok.Mapperly.Abstractions;

namespace liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Mappers
{

    [Mapper]
    internal static partial class ImageDbMapper
    {
        internal static partial ImageDbEntity ToEntity(this ImageCloudinary image);
        internal static partial ImageCloudinary ToDomain(this ImageDbEntity entity);
    }
}
