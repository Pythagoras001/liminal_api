using liminal_api.Modules.User.Application.Port.Out;
using liminal_api.Modules.User.Domain;
using liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence.Mappers;
using liminal_api.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence
{
    public class UserRespositoryAdapter : IUserRespositoryPort
    {
        private readonly AppDbContext _db;

        public UserRespositoryAdapter(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserRegister?> findById(string userId)
        {
            UserDbEntity? userDbEntity = await _db.Set<UserDbEntity>()
                .AsNoTracking()
                .Include(u => u.ImgProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return userDbEntity?.ToDomain();
        }

        public async Task<UserRegister> save(UserRegister userRegister)
        {
            userRegister.initDefaults();
            UserDbEntity userDbEntity = UserDbMapper.ToEntity(userRegister);
            _db.Set<UserDbEntity>().Add(userDbEntity);
            await _db.SaveChangesAsync();

            return UserDbMapper.ToDomain(userDbEntity);
        }

        public async Task<UserRegister> update(UserRegister userRegister)
        {
            UserDbEntity userDbEntity = UserDbMapper.ToEntity(userRegister);
            _db.Set<UserDbEntity>().Update(userDbEntity);
            await _db.SaveChangesAsync();
            return UserDbMapper.ToDomain(userDbEntity);
        }
    }
}
