using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Recursos;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities.Recursos;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecursosController : ControllerBase
{
    private readonly IRecursoService _recursoService;

    public RecursosController(IRecursoService recursoService)
    {
        _recursoService = recursoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var recursos = await _recursoService.GetAllAsync();
        return Ok(recursos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var recurso = await _recursoService.GetByIdAsync(id);
        if (recurso == null) return NotFound(new { message = "Recurso no encontrado." });
        return Ok(recurso);
    }

    [HttpGet("estado/{estado}")]
    public async Task<IActionResult> GetByEstado(EstadoRecurso estado)
    {
        var recursos = await _recursoService.GetByEstadoAsync(estado);
        return Ok(recursos);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SaveRecursoDto dto)
    {
        var result = await _recursoService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRecursoDto dto)
    {
        var result = await _recursoService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(string id, [FromBody] EstadoRecurso estado)
    {
        var result = await _recursoService.CambiarEstadoAsync(id, estado);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _recursoService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
