using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Auditoria;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var registros = await _auditoriaService.GetAllAsync();
        return Ok(registros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var registro = await _auditoriaService.GetByIdAsync(id);
        if (registro == null) return NotFound(new { message = "Registro no encontrado." });
        return Ok(registro);
    }

    [HttpGet("fecha")]
    public async Task<IActionResult> GetByFecha([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        var registros = await _auditoriaService.GetByFechaAsync(desde, hasta);
        return Ok(registros);
    }

    [HttpGet("usuario/{usuario}")]
    public async Task<IActionResult> GetByUsuario(string usuario)
    {
        var registros = await _auditoriaService.GetByUsuarioAsync(usuario);
        return Ok(registros);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SaveAuditoriaDto dto)
    {
        var result = await _auditoriaService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
