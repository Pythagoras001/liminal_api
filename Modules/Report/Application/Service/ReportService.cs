using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Domain;
using liminal_api.Modules.LevelClass.Application.Port.In;
using liminal_api.Modules.Report.Application.Command;
using liminal_api.Modules.Report.Application.Port.In;
using liminal_api.Modules.Report.Application.Port.Out;
using liminal_api.Modules.Report.Domain;
using ReportEntity = liminal_api.Modules.Report.Domain.Report;

namespace liminal_api.Modules.Report.Application.Service
{
    public class ReportService : IReportUseCase
    {
        private readonly IReportRepositoryPort _repository;
        private readonly ICloudinaryImageRepositoryPort _imageRepository;
        private readonly ILevelClassUseCase _levelClassUseCase;

        public ReportService(
            IReportRepositoryPort repository,
            ICloudinaryImageRepositoryPort imageRepository,
            ILevelClassUseCase levelClassUseCase)
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _levelClassUseCase = levelClassUseCase;
        }

        public async Task<ReportEntity> Create(CreateReportCommand command)
        {
            await _levelClassUseCase.FindById(command.LevelClassId);

            if (command.GaleryEvidencesDescriptions.Count != command.GaleryEvidences.Count)
                throw new ArgumentException(
                    "galeryEvidencesDescriptions and galeryEvidences must have the same length.");

            ImageCloudinary principalImage = await _imageRepository.UploadAsync(
                command.PrincipalEvidence, Guid.NewGuid().ToString());
            var principalEvidence = new Evidence(
                null, principalImage, command.PrincipalEvidenceDescription);

            var gallery = new List<Evidence>();
            for (int i = 0; i < command.GaleryEvidences.Count; i++)
            {
                ImageCloudinary image = await _imageRepository.UploadAsync(
                    command.GaleryEvidences[i], Guid.NewGuid().ToString());
                gallery.Add(new Evidence(
                    null, image, command.GaleryEvidencesDescriptions[i]));
            }

            var report = new ReportEntity(
                null,
                command.AuthorId,
                command.Title,
                command.Nivel,
                command.LevelClassId,
                command.Description,
                principalEvidence,
                DateTime.UtcNow,
                gallery);

            return await _repository.Create(report);
        }

        public async Task<ReportEntity> Rate(int reportId, string userId, bool liked)
        {
            ReportEntity report = await FindReport(reportId);
            report.Rate(userId, liked);
            return await _repository.Update(report);
        }

        public async Task<ReportEntity> Update(UpdateReportCommand command)
        {
            ReportEntity report = await FindReport(command.Id);
            if (report.AuthorId != command.UserId)
                throw new UnauthorizedAccessException("You are not allowed to update this report.");

            if (command.LevelClassId.HasValue)
            {
                await _levelClassUseCase.FindById(command.LevelClassId.Value);
                report.ChangeLevelClassId(command.LevelClassId.Value);
            }

            if (command.Title != null)
                report.ChangeTitle(command.Title);
            if (command.Nivel != null)
                report.ChangeNivel(command.Nivel);
            if (command.Description != null)
                report.ChangeDescription(command.Description);

            return await _repository.Update(report);
        }

        public async Task Delete(int reportId, string userId)
        {
            ReportEntity report = await FindReport(reportId);
            if (report.AuthorId != userId)
                throw new UnauthorizedAccessException("You are not allowed to delete this report.");

            await _repository.Delete(reportId);
        }

        private async Task<ReportEntity> FindReport(int id)
        {
            return await _repository.FindById(id)
                ?? throw new KeyNotFoundException($"Report with id {id} not found.");
        }
    }
}
