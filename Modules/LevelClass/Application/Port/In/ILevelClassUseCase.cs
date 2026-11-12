using liminal_api.Modules.LevelClass.Application.Command;
using LevelClassEntity = liminal_api.Modules.LevelClass.Domain.LevelClass;

namespace liminal_api.Modules.LevelClass.Application.Port.In
{
    public interface ILevelClassUseCase
    {
        Task<IReadOnlyList<LevelClassEntity>> FindAll();
        Task<LevelClassEntity> FindById(int id);
        Task<LevelClassEntity> Create(CreateLevelClassCommand command);
        Task<LevelClassEntity> Update(UpdateLevelClassCommand command);
    }
}
