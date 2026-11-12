using liminal_api.Modules.User.Application.Port.Out;
using liminal_api.Modules.User.Domain;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Mappers;
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

        public async Task<UserRegister?> findById(long userId)
        {
            UserDbEntity? userDbEntity = await _db.Set<UserDbEntity>()
                .AsNoTracking()
                .Include(u => u.ImgProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return userDbEntity?.ToDomain();
        }

        public async Task<UserRegister?> findByEmail(string email)
        {
            UserDbEntity? userDbEntity = await _db.Set<UserDbEntity>()
                .AsNoTracking()
                .Include(u => u.ImgProfile)
                .FirstOrDefaultAsync(u => u.Email == email);

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
            UserDbEntity userDbEntity = await _db.Set<UserDbEntity>()
                .FirstOrDefaultAsync(user => user.Id == userRegister.Id)
                ?? throw new KeyNotFoundException($"User with id {userRegister.Id} not found.");

            userDbEntity.UserName = userRegister.UserName;
            userDbEntity.Email = userRegister.Email;
            userDbEntity.Password = userRegister.Password;

            if (userRegister.ImgProfile != null)
            {
                if (string.IsNullOrWhiteSpace(userRegister.ImgProfile.Id))
                {
                    ImageDbEntity imageDbEntity = ImageDbMapper.ToEntity(
                        userRegister.ImgProfile.initDefaults());
                    imageDbEntity.Id = Guid.NewGuid().ToString();
                    userDbEntity.ImgProfile = imageDbEntity;
                    userDbEntity.ImgProfileId = imageDbEntity.Id;
                    _db.Set<ImageDbEntity>().Add(imageDbEntity);
                }
                else if (userDbEntity.ImgProfileId != userRegister.ImgProfile.Id)
                {
                    userDbEntity.ImgProfileId = userRegister.ImgProfile.Id;
                    userDbEntity.ImgProfile = await _db.Set<ImageDbEntity>()
                        .FindAsync(userRegister.ImgProfile.Id)
                        ?? throw new KeyNotFoundException(
                            $"Profile image with id {userRegister.ImgProfile.Id} not found.");
                }
            }

            await _db.SaveChangesAsync();

            UserDbEntity updatedUser = await _db.Set<UserDbEntity>()
                .AsNoTracking()
                .Include(user => user.ImgProfile)
                .FirstAsync(user => user.Id == userRegister.Id);
            return UserDbMapper.ToDomain(updatedUser);
        }
    }
}
