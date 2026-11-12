using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.LevelClass.Application.Command;
using liminal_api.Modules.LevelClass.Application.Port.In;
using liminal_api.Modules.LevelClass.Application.Port.Out;
using LevelClassEntity = liminal_api.Modules.LevelClass.Domain.LevelClass;

namespace liminal_api.Modules.LevelClass.Application.Service
{
    public class LevelClassService : ILevelClassUseCase
    {
        private readonly ILevelClassRepositoryPort _repository;
        private readonly ICloudinaryImageRepositoryPort _imageRepository;

        public LevelClassService(
            ILevelClassRepositoryPort repository,
            ICloudinaryImageRepositoryPort imageRepository)
        {
            _repository = repository;
            _imageRepository = imageRepository;
        }

        public Task<IReadOnlyList<LevelClassEntity>> FindAll() => _repository.FindAll();

        public async Task<LevelClassEntity> FindById(int id)
        {
            return await _repository.FindById(id)
                ?? throw new KeyNotFoundException($"Level class with id {id} not found.");
        }

        public async Task<LevelClassEntity> Create(CreateLevelClassCommand command)
        {
            ImageCloudinary icon = await _imageRepository.UploadAsync(
                command.Icon,
                Guid.NewGuid().ToString());

            var levelClass = new LevelClassEntity(
                command.Type,
                command.ClassNumber,
                command.SecurityLevel,
                command.Legitimacy,
                command.DangerLevel,
                icon,
                command.Description);

            return await _repository.Save(levelClass);
        }

        public async Task<LevelClassEntity> Update(UpdateLevelClassCommand command)
        {
            LevelClassEntity levelClass = await _repository.FindById(command.Id)
                ?? throw new KeyNotFoundException($"Level class with id {command.Id} not found.");

            if (command.Type.HasValue)
                levelClass.ChangeType(command.Type.Value);
            if (!string.IsNullOrEmpty(command.ClassNumber))
                levelClass.ChangeClassNumber(command.ClassNumber);
            if (!string.IsNullOrEmpty(command.SecurityLevel))
                levelClass.ChangeSecurityLevel(command.SecurityLevel);
            if (command.Legitimacy.HasValue)
                levelClass.ChangeLegitimacy(command.Legitimacy.Value);
            if (!string.IsNullOrEmpty(command.DangerLevel))
                levelClass.ChangeDangerLevel(command.DangerLevel);
            if (!string.IsNullOrEmpty(command.Description))
                levelClass.ChangeDescription(command.Description);

            string? oldPublicId = null;
            if (command.Icon != null)
            {
                oldPublicId = levelClass.IconImage.PublicId;
                ImageCloudinary newIcon = await _imageRepository.UploadAsync(
                    command.Icon,
                    Guid.NewGuid().ToString());
                levelClass.ChangeIconImage(newIcon);
            }

            LevelClassEntity updated = await _repository.Update(levelClass);
            if (!string.IsNullOrEmpty(oldPublicId))
                await _imageRepository.deleteImage(oldPublicId);

            return updated;
        }
    }
}
