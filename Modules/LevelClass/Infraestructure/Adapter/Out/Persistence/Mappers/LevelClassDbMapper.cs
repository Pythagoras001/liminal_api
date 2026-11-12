using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Mappers;
using liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.LevelClass.Domain.Enums;
using LevelClassEntity = liminal_api.Modules.LevelClass.Domain.LevelClass;

namespace liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence.Mappers
{
    internal static class LevelClassDbMapper
    {
        internal static LevelClassEntity ToDomain(LevelClassDbEntity entity)
        {
            var image = new ImageCloudinary(
                entity.IconImage.Id,
                entity.IconImage.PublicId,
                entity.IconImage.OriginalUrl,
                entity.IconImage.ThumbnailUrl,
                entity.IconImage.MediumUrl,
                entity.IconImage.CreatedAt);

            return new LevelClassEntity(
                Enum.Parse<SurvivalClassType>(entity.Type),
                entity.ClassNumber,
                entity.SecurityLevel,
                Enum.Parse<Legitimacy>(entity.Legitimacy),
                entity.DangerLevel,
                image,
                entity.Description,
                entity.Id);
        }

        internal static LevelClassDbEntity ToEntity(LevelClassEntity levelClass)
        {
            ImageCloudinary image = levelClass.IconImage.initDefaults();
            ImageDbEntity imageEntity = ImageDbMapper.ToEntity(image);
            imageEntity.Id ??= Guid.NewGuid().ToString();

            return new LevelClassDbEntity
            {
                Id = levelClass.Id ?? 0,
                Type = levelClass.Type.ToString(),
                ClassNumber = levelClass.ClassNumber,
                SecurityLevel = levelClass.SecurityLevel,
                Legitimacy = levelClass.Legitimacy.ToString(),
                DangerLevel = levelClass.DangerLevel,
                IconImageId = imageEntity.Id,
                IconImage = imageEntity,
                Description = levelClass.Description
            };
        }
    }
}
