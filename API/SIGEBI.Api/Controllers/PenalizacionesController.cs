using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Penalizaciones;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PenalizacionesController : ControllerBase
{
    private readonly IPenalizacionService _penalizacionService;

    public PenalizacionesController(IPenalizacionService penalizacionService)
    {
        _penalizacionService = penalizacionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PenalizacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PenalizacionDto>>> GetAll()
        => Ok(await _penalizacionService.GetAllAsync());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PenalizacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PenalizacionDto>> GetById(string id)
    {
        var penalizacion = await _penalizacionService.GetByIdAsync(id);
        if (penalizacion == null) return NotFound(new { message = "Penalización no encontrada." });
        return Ok(penalizacion);
    }

    [HttpGet("activas/{usuarioId}")]
    [ProducesResponseType(typeof(IEnumerable<PenalizacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PenalizacionDto>>> GetActivasByUsuario(string usuarioId)
        => Ok(await _penalizacionService.GetActivasByUsuarioAsync(usuarioId));

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Save([FromBody] SavePenalizacionDto dto)
    {
        var result = await _penalizacionService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return StatusCode(StatusCodes.Status201Created, new { message = result.Message });
    }

    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] UpdatePenalizacionDto dto)
    {
        var result = await _penalizacionService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id}/resolver")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Resolver(string id)
    {
        var result = await _penalizacionService.ResolverAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _penalizacionService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
