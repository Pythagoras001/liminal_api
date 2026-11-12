using LevelClassEntity = liminal_api.Modules.LevelClass.Domain.LevelClass;

namespace liminal_api.Modules.LevelClass.Application.Port.Out
{
    public interface ILevelClassRepositoryPort
    {
        Task<IReadOnlyList<LevelClassEntity>> FindAll();
        Task<LevelClassEntity?> FindById(int id);
        Task<LevelClassEntity> Save(LevelClassEntity levelClass);
        Task<LevelClassEntity> Update(LevelClassEntity levelClass);
    }
}
