using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Entities;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Persistence.Mappers;
using liminal_api.Modules.Report.Domain;
using liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Entities;
using ReportEntity = liminal_api.Modules.Report.Domain.Report;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence.Mappers
{
    internal static class ReportDbMapper
    {
        internal static ReportEntity ToDomain(ReportDbEntity entity)
        {
            var likes = entity.Likes.Select(like => new ReportLike(
                like.UserId.ToString(),
                like.ReportId,
                like.CreatedAt));

            return new ReportEntity(
                entity.Id,
                entity.AuthorId.ToString(),
                entity.Title,
                entity.Nivel,
                entity.LevelClassId,
                entity.Description,
                EvidenceToDomain(entity.PrincipalEvidence),
                entity.CreatedAt,
                entity.GaleryEvidences.Select(EvidenceToDomain),
                likes);
        }

        internal static ReportDbEntity ToEntity(ReportEntity report)
        {
            var entity = new ReportDbEntity
            {
                Id = report.Id ?? 0,
                AuthorId = long.Parse(report.AuthorId),
                Title = report.Title,
                Nivel = report.Nivel,
                LevelClassId = report.LevelClassId,
                Description = report.Description,
                CreatedAt = report.CreatedAt,
                PrincipalEvidence = EvidenceToEntity(report.PrincipalEvidence),
                GaleryEvidences = report.GaleryEvidences
                    .Select(EvidenceToEntity)
                    .ToList()
            };

            entity.PrincipalEvidenceId = entity.PrincipalEvidence.Id;
            return entity;
        }

        internal static EvidenceDbEntity EvidenceToEntity(Evidence evidence)
        {
            ImageCloudinary image = evidence.Image.initDefaults();
            ImageDbEntity imageEntity = ImageDbMapper.ToEntity(image);
            imageEntity.Id ??= Guid.NewGuid().ToString();

            return new EvidenceDbEntity
            {
                Id = evidence.Id ?? Guid.NewGuid().ToString(),
                Description = evidence.Description,
                ImageId = imageEntity.Id,
                Image = imageEntity
            };
        }

        private static Evidence EvidenceToDomain(EvidenceDbEntity evidence)
        {
            var image = new ImageCloudinary(
                evidence.Image.Id,
                evidence.Image.PublicId,
                evidence.Image.OriginalUrl,
                evidence.Image.ThumbnailUrl,
                evidence.Image.MediumUrl,
                evidence.Image.CreatedAt);

            return new Evidence(evidence.Id, image, evidence.Description);
        }
    }
}
