using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Notificaciones;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificacionesController : ControllerBase
{
    private readonly INotificacionService _notificacionService;

    public NotificacionesController(INotificacionService notificacionService)
    {
        _notificacionService = notificacionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notificaciones = await _notificacionService.GetAllAsync();
        return Ok(notificaciones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var notificacion = await _notificacionService.GetByIdAsync(id);
        if (notificacion == null) return NotFound(new { message = "Notificación no encontrada." });
        return Ok(notificacion);
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> GetByUsuario(string usuarioId)
    {
        var notificaciones = await _notificacionService.GetByUsuarioAsync(usuarioId);
        return Ok(notificaciones);
    }

    [HttpPost("enviar")]
    public async Task<IActionResult> Enviar([FromBody] SaveNotificacionDto dto)
    {
        var result = await _notificacionService.EnviarAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPost("vencimientos-proximos")]
    public async Task<IActionResult> NotificarVencimientosProximos()
    {
        await _notificacionService.NotificarVencimientosProximosAsync();
        return Ok(new { message = "Notificaciones de vencimientos próximos enviadas." });
    }

    [HttpPost("prestamos-vencidos")]
    public async Task<IActionResult> NotificarPrestamosVencidos()
    {
        await _notificacionService.NotificarPrestamosVencidosAsync();
        return Ok(new { message = "Notificaciones de préstamos vencidos enviadas." });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateNotificacionDto dto)
    {
        var result = await _notificacionService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _notificacionService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
