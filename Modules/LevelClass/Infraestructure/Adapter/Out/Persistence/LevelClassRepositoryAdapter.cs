using liminal_api.Modules.LevelClass.Application.Port.Out;
using liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Mappers;
using liminal_api.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using LevelClassEntity = liminal_api.Modules.LevelClass.Domain.LevelClass;

namespace liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence
{
    public class LevelClassRepositoryAdapter : ILevelClassRepositoryPort
    {
        private readonly AppDbContext _db;

        public LevelClassRepositoryAdapter(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<LevelClassEntity>> FindAll()
        {
            var entities = await _db.Set<LevelClassDbEntity>()
                .AsNoTracking()
                .Include(levelClass => levelClass.IconImage)
                .ToListAsync();

            return entities.Select(LevelClassDbMapper.ToDomain).ToList();
        }

        public async Task<LevelClassEntity?> FindById(int id)
        {
            var entity = await _db.Set<LevelClassDbEntity>()
                .AsNoTracking()
                .Include(levelClass => levelClass.IconImage)
                .FirstOrDefaultAsync(levelClass => levelClass.Id == id);

            return entity == null ? null : LevelClassDbMapper.ToDomain(entity);
        }

        public async Task<LevelClassEntity> Save(LevelClassEntity levelClass)
        {
            var entity = LevelClassDbMapper.ToEntity(levelClass);
            _db.Set<LevelClassDbEntity>().Add(entity);
            await _db.SaveChangesAsync();
            return LevelClassDbMapper.ToDomain(entity);
        }

        public async Task<LevelClassEntity> Update(LevelClassEntity levelClass)
        {
            var entity = await _db.Set<LevelClassDbEntity>()
                .Include(current => current.IconImage)
                .FirstOrDefaultAsync(current => current.Id == levelClass.Id);

            if (entity == null)
                throw new KeyNotFoundException($"Level class with id {levelClass.Id} not found.");

            var updated = LevelClassDbMapper.ToEntity(levelClass);
            entity.Type = updated.Type;
            entity.ClassNumber = updated.ClassNumber;
            entity.SecurityLevel = updated.SecurityLevel;
            entity.Legitimacy = updated.Legitimacy;
            entity.DangerLevel = updated.DangerLevel;
            entity.Description = updated.Description;

            if (entity.IconImageId != updated.IconImageId)
            {
                entity.IconImageId = updated.IconImageId;
                entity.IconImage = updated.IconImage;
                _db.Entry(updated.IconImage).State = EntityState.Added;
            }

            await _db.SaveChangesAsync();
            return LevelClassDbMapper.ToDomain(entity);
        }
    }
}
