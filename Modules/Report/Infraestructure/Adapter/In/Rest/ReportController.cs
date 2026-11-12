using System.Security.Claims;
using liminal_api.Modules.Report.Application.Command;
using liminal_api.Modules.Report.Application.ReadModel;
using liminal_api.Modules.Report.Application.Service;
using liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportEntity = liminal_api.Modules.Report.Domain.Report;

namespace liminal_api.Modules.Report.Infraestructure.Adapter.In.Rest
{
    [ApiController]
    [Route("report")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;
        private readonly ReportQueryService _reportQueryService;

        public ReportController(
            ReportService reportService,
            ReportQueryService reportQueryService)
        {
            _reportService = reportService;
            _reportQueryService = reportQueryService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginatedResponseDto<ReportDetailResponseDto>>> FindAll(
            [FromQuery] FindAllReportsQueryDto query)
        {
            var result = await _reportQueryService.FindAll(query.Page ?? 1);
            return Ok(ToResponse(result));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<PaginatedResponseDto<ReportDetailResponseDto>>> FindByAuthor(
            [FromQuery] FindAllReportsQueryDto query)
        {
            string? userId = CurrentUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated." });

            try
            {
                var result = await _reportQueryService.FindByAuthor(userId, query.Page ?? 1);
                return Ok(ToResponse(result));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found." });
            }
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReportDetailResponseDto>> FindById(int id)
        {
            try
            {
                return Ok(ReportDetailResponseDto.FromReadModel(
                    await _reportQueryService.FindById(id)));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Report with id {id} not found." });
            }
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ReportResponseDto>> Create(
            [FromForm] CreateReportRequestDto request)
        {
            string? userId = CurrentUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated." });
            if (request.PrincipalEvidence == null || request.PrincipalEvidence.Length == 0)
                return BadRequest(new { message = "principalEvidence is required" });

            using var principalStream = request.PrincipalEvidence.OpenReadStream();
            var galleryFiles = request.GaleryEvidences ?? new List<IFormFile>();
            var galleryStreams = galleryFiles.Select(file => file.OpenReadStream()).ToList();

            try
            {
                var command = new CreateReportCommand(
                    userId,
                    request.Title,
                    request.Nivel,
                    request.LevelClassId,
                    request.Description,
                    request.PrincipalEvidenceDescription,
                    principalStream,
                    request.GaleryEvidencesDescriptions ?? Array.Empty<string>(),
                    galleryStreams);

                ReportEntity report = await _reportService.Create(command);
                return StatusCode(
                    StatusCodes.Status201Created,
                    ReportResponseDto.FromDomain(report));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                foreach (var stream in galleryStreams)
                    await stream.DisposeAsync();
            }
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult<ReportResponseDto>> Rate(
            [FromBody] RateReportRequestDto request)
        {
            string? userId = CurrentUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated." });

            try
            {
                ReportEntity report = await _reportService.Rate(
                    request.ReportId, userId, request.Liked!.Value);
                return Ok(ReportResponseDto.FromDomain(report));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Report with id {request.ReportId} not found." });
            }
        }

        [Authorize]
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ReportResponseDto>> Update(
            int id,
            [FromBody] UpdateReportRequestDto request)
        {
            string? userId = CurrentUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated." });

            try
            {
                var command = new UpdateReportCommand(
                    id, userId, request.Title, request.Nivel,
                    request.LevelClassId, request.Description);
                ReportEntity report = await _reportService.Update(command);
                return Ok(ReportResponseDto.FromDomain(report));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            string? userId = CurrentUserId();
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated." });

            try
            {
                await _reportService.Delete(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        private string? CurrentUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        private static PaginatedResponseDto<ReportDetailResponseDto> ToResponse(
            PaginatedResult<ReportDetailReadModel> result) =>
            new(
                result.Data.Select(ReportDetailResponseDto.FromReadModel).ToList(),
                result.Page,
                result.PageSize,
                result.Total,
                result.TotalPages);
    }
}
