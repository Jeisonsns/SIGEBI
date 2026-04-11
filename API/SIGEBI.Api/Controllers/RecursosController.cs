using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Recursos;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities.Recursos;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RecursosController : ControllerBase
{
    private readonly IRecursoService _recursoService;

    public RecursosController(IRecursoService recursoService)
    {
        _recursoService = recursoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RecursoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RecursoDto>>> GetAll()
    {
        var recursos = await _recursoService.GetAllAsync();
        return Ok(recursos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RecursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RecursoDto>> GetById(string id)
    {
        var recurso = await _recursoService.GetByIdAsync(id);
        if (recurso == null) return NotFound(new { message = "Recurso no encontrado." });
        return Ok(recurso);
    }

    [HttpGet("estado/{estado}")]
    [ProducesResponseType(typeof(IEnumerable<RecursoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RecursoDto>>> GetByEstado(EstadoRecurso estado)
    {
        var recursos = await _recursoService.GetByEstadoAsync(estado);
        return Ok(recursos);
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Save([FromBody] SaveRecursoDto dto)
    {
        var result = await _recursoService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return StatusCode(StatusCodes.Status201Created, new { message = result.Message });
    }

    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] UpdateRecursoDto dto)
    {
        var result = await _recursoService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id}/estado")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CambiarEstado(string id, [FromBody] EstadoRecurso estado)
    {
        var result = await _recursoService.CambiarEstadoAsync(id, estado);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _recursoService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
