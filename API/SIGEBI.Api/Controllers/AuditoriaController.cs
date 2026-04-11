using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Auditoria;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuditoriaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuditoriaDto>>> GetAll()
        => Ok(await _auditoriaService.GetAllAsync());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AuditoriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuditoriaDto>> GetById(string id)
    {
        var registro = await _auditoriaService.GetByIdAsync(id);
        if (registro == null) return NotFound(new { message = "Registro no encontrado." });
        return Ok(registro);
    }

    [HttpGet("fecha")]
    [ProducesResponseType(typeof(IEnumerable<AuditoriaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuditoriaDto>>> GetByFecha(
        [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        => Ok(await _auditoriaService.GetByFechaAsync(desde, hasta));

    [HttpGet("usuario/{usuario}")]
    [ProducesResponseType(typeof(IEnumerable<AuditoriaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuditoriaDto>>> GetByUsuario(string usuario)
        => Ok(await _auditoriaService.GetByUsuarioAsync(usuario));

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Save([FromBody] SaveAuditoriaDto dto)
    {
        var result = await _auditoriaService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return StatusCode(StatusCodes.Status201Created, new { message = result.Message });
    }
}
