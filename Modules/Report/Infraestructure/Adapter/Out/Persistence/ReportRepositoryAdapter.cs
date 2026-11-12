using liminal_api.Modules.Report.Application.Port.Out;
using liminal_api.Modules.Report.Domain;
using liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Mappers;
using liminal_api.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ReportEntity = liminal_api.Modules.Report.Domain.Report;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence
{
    public class ReportRepositoryAdapter : IReportRepositoryPort
    {
        private readonly AppDbContext _db;

        public ReportRepositoryAdapter(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ReportEntity> Create(ReportEntity report)
        {
            var entity = ReportDbMapper.ToEntity(report);
            _db.Set<ReportDbEntity>().Add(entity);
            await _db.SaveChangesAsync();
            return ReportDbMapper.ToDomain(entity);
        }

        public async Task<IReadOnlyList<ReportEntity>> FindAll(int skip, int take)
        {
            var entities = await QueryWithRelations()
                .AsNoTracking()
                .OrderByDescending(report => report.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            return entities.Select(ReportDbMapper.ToDomain).ToList();
        }

        public async Task<IReadOnlyList<ReportEntity>> FindByAuthor(
            string authorId, int skip, int take)
        {
            if (!long.TryParse(authorId, out long userId))
                return Array.Empty<ReportEntity>();

            var entities = await QueryWithRelations()
                .AsNoTracking()
                .Where(report => report.AuthorId == userId)
                .OrderByDescending(report => report.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            return entities.Select(ReportDbMapper.ToDomain).ToList();
        }

        public async Task<ReportEntity?> FindById(int id)
        {
            var entity = await QueryWithRelations()
                .AsNoTracking()
                .FirstOrDefaultAsync(report => report.Id == id);
            return entity == null ? null : ReportDbMapper.ToDomain(entity);
        }

        public Task<int> Count() => _db.Set<ReportDbEntity>().CountAsync();

        public Task<int> CountByAuthor(string authorId)
        {
            if (!long.TryParse(authorId, out long userId))
                return Task.FromResult(0);
            return _db.Set<ReportDbEntity>().CountAsync(report => report.AuthorId == userId);
        }

        public async Task<ReportEntity> Update(ReportEntity report)
        {
            int id = report.Id
                ?? throw new InvalidOperationException("Cannot update a report without an id.");

            var entity = await _db.Set<ReportDbEntity>()
                .Include(current => current.Likes)
                .FirstOrDefaultAsync(current => current.Id == id)
                ?? throw new KeyNotFoundException($"Report with id {id} not found.");

            entity.Title = report.Title;
            entity.Nivel = report.Nivel;
            entity.LevelClassId = report.LevelClassId;
            entity.Description = report.Description;

            var currentUserIds = report.Likes
                .Select(like => long.Parse(like.UserId))
                .ToHashSet();
            foreach (var existingLike in entity.Likes.ToList())
            {
                if (!currentUserIds.Contains(existingLike.UserId))
                    _db.Set<ReportLikeDbEntity>().Remove(existingLike);
            }

            var existingUserIds = entity.Likes.Select(like => like.UserId).ToHashSet();
            foreach (ReportLike like in report.Likes)
            {
                long userId = long.Parse(like.UserId);
                if (!existingUserIds.Contains(userId))
                {
                    entity.Likes.Add(new ReportLikeDbEntity
                    {
                        UserId = userId,
                        ReportId = id,
                        CreatedAt = like.CreatedAt
                    });
                }
            }

            await _db.SaveChangesAsync();
            var updated = await QueryWithRelations()
                .AsNoTracking()
                .FirstAsync(current => current.Id == id);
            return ReportDbMapper.ToDomain(updated);
        }

        public async Task Delete(int id)
        {
            var entity = await _db.Set<ReportDbEntity>()
                .FirstOrDefaultAsync(report => report.Id == id)
                ?? throw new KeyNotFoundException($"Report with id {id} not found.");

            _db.Set<ReportDbEntity>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        private IQueryable<ReportDbEntity> QueryWithRelations() =>
            _db.Set<ReportDbEntity>()
                .Include(report => report.PrincipalEvidence)
                    .ThenInclude(evidence => evidence.Image)
                .Include(report => report.GaleryEvidences)
                    .ThenInclude(evidence => evidence.Image)
                .Include(report => report.Likes);
    }
}
