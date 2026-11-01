using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Mappers;
using liminal_api.Shared.Infrastructure.Persistence;

namespace liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence
{
    public class ImageRepository : IImageRepositoryPort
    {

        private readonly AppDbContext _db;

        public ImageRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ImageCloudinary> save(ImageCloudinary image)
        {
            image.initDefaults();
            ImageDbEntity entity = ImageDbMapper.ToEntity(image);
            _db.Set<ImageDbEntity>().Add(entity);
            await _db.SaveChangesAsync();

            return ImageDbMapper.ToDomain(entity);
        }
    }
}
