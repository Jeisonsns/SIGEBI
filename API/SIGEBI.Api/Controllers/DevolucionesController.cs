using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Devoluciones;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevolucionesController : ControllerBase
{
    private readonly IDevolucionService _devolucionService;

    public DevolucionesController(IDevolucionService devolucionService)
    {
        _devolucionService = devolucionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var devoluciones = await _devolucionService.GetAllAsync();
        return Ok(devoluciones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var devolucion = await _devolucionService.GetByIdAsync(id);
        if (devolucion == null) return NotFound(new { message = "Devolución no encontrada." });
        return Ok(devolucion);
    }

    [HttpPost("procesar/{prestamoId}")]
    public async Task<IActionResult> Procesar(string prestamoId)
    {
        var result = await _devolucionService.ProcesarDevolucionAsync(prestamoId);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateDevolucionDto dto)
    {
        var result = await _devolucionService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _devolucionService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
