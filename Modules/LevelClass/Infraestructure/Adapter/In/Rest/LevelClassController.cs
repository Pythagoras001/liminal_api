using liminal_api.Modules.LevelClass.Application.Command;
using liminal_api.Modules.LevelClass.Application.Port.In;
using liminal_api.Modules.LevelClass.Infraestructure.Adapter.In.Rest.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace liminal_api.Modules.LevelClass.Infraestructure.Adapter.In.Rest
{
    [ApiController]
    [Route("class")]
    public class LevelClassController : ControllerBase
    {
        private readonly ILevelClassUseCase _levelClassUseCase;

        public LevelClassController(ILevelClassUseCase levelClassUseCase)
        {
            _levelClassUseCase = levelClassUseCase;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<LevelClassResponseDto>>> FindAll()
        {
            var classes = await _levelClassUseCase.FindAll();
            return Ok(classes.Select(LevelClassResponseDto.FromDomain));
        }

        [AllowAnonymous]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<LevelClassResponseDto>> Create(
            [FromForm] CreateLevelClassRequestDto request)
        {
            if (request.Icon == null || request.Icon.Length == 0)
                return BadRequest(new { message = "icon is required" });

            await using var icon = request.Icon.OpenReadStream();
            var command = new CreateLevelClassCommand(
                request.Type!.Value,
                request.ClassNumber,
                request.SecurityLevel,
                request.Legitimacy!.Value,
                request.DangerLevel,
                request.Description,
                icon);

            var created = await _levelClassUseCase.Create(command);
            return Ok(LevelClassResponseDto.FromDomain(created));
        }

        [Authorize]
        [HttpPatch("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<LevelClassResponseDto>> Update(
            int id,
            [FromForm] UpdateLevelClassRequestDto request)
        {
            await using var icon = request.Icon?.OpenReadStream();
            var command = new UpdateLevelClassCommand(
                id,
                request.Type,
                request.ClassNumber,
                request.SecurityLevel,
                request.Legitimacy,
                request.DangerLevel,
                request.Description,
                icon);

            try
            {
                var updated = await _levelClassUseCase.Update(command);
                return Ok(LevelClassResponseDto.FromDomain(updated));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Level class with id {id} not found." });
            }
        }
    }
}
