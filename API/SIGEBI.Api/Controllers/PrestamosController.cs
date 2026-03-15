using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Prestamos;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoService _prestamoService;

    public PrestamosController(IPrestamoService prestamoService)
    {
        _prestamoService = prestamoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var prestamos = await _prestamoService.GetAllAsync();
        return Ok(prestamos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var prestamo = await _prestamoService.GetByIdAsync(id);
        if (prestamo == null) return NotFound(new { message = "Préstamo no encontrado." });
        return Ok(prestamo);
    }

    [HttpGet("activos/{usuarioId}")]
    public async Task<IActionResult> GetActivosByUsuario(string usuarioId)
    {
        var prestamos = await _prestamoService.GetActivosByUsuarioAsync(usuarioId);
        return Ok(prestamos);
    }

    [HttpGet("vencidos")]
    public async Task<IActionResult> GetVencidos()
    {
        var prestamos = await _prestamoService.GetVencidosAsync();
        return Ok(prestamos);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SavePrestamoDto dto)
    {
        var result = await _prestamoService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePrestamoDto dto)
    {
        var result = await _prestamoService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id}/renovar")]
    public async Task<IActionResult> Renovar(string id)
    {
        var result = await _prestamoService.RenovarAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _prestamoService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
