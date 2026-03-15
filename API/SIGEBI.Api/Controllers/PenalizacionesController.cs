using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Penalizaciones;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PenalizacionesController : ControllerBase
{
    private readonly IPenalizacionService _penalizacionService;

    public PenalizacionesController(IPenalizacionService penalizacionService)
    {
        _penalizacionService = penalizacionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var penalizaciones = await _penalizacionService.GetAllAsync();
        return Ok(penalizaciones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var penalizacion = await _penalizacionService.GetByIdAsync(id);
        if (penalizacion == null) return NotFound(new { message = "Penalización no encontrada." });
        return Ok(penalizacion);
    }

    [HttpGet("activas/{usuarioId}")]
    public async Task<IActionResult> GetActivasByUsuario(string usuarioId)
    {
        var penalizaciones = await _penalizacionService.GetActivasByUsuarioAsync(usuarioId);
        return Ok(penalizaciones);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SavePenalizacionDto dto)
    {
        var result = await _penalizacionService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePenalizacionDto dto)
    {
        var result = await _penalizacionService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id}/resolver")]
    public async Task<IActionResult> Resolver(string id)
    {
        var result = await _penalizacionService.ResolverAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _penalizacionService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
