using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Notificaciones;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class NotificacionesController : ControllerBase
{
    private readonly INotificacionService _notificacionService;

    public NotificacionesController(INotificacionService notificacionService)
    {
        _notificacionService = notificacionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotificacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetAll()
        => Ok(await _notificacionService.GetAllAsync());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(NotificacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotificacionDto>> GetById(string id)
    {
        var notificacion = await _notificacionService.GetByIdAsync(id);
        if (notificacion == null) return NotFound(new { message = "Notificación no encontrada." });
        return Ok(notificacion);
    }

    [HttpGet("usuario/{usuarioId}")]
    [ProducesResponseType(typeof(IEnumerable<NotificacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetByUsuario(string usuarioId)
        => Ok(await _notificacionService.GetByUsuarioAsync(usuarioId));

    [HttpPost("enviar")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Enviar([FromBody] SaveNotificacionDto dto)
    {
        var result = await _notificacionService.EnviarAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return StatusCode(StatusCodes.Status201Created, new { message = result.Message });
    }

    [HttpPost("vencimientos-proximos")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult> NotificarVencimientosProximos()
    {
        await _notificacionService.NotificarVencimientosProximosAsync();
        return Ok(new { message = "Notificaciones de vencimientos próximos enviadas." });
    }

    [HttpPost("prestamos-vencidos")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult> NotificarPrestamosVencidos()
    {
        await _notificacionService.NotificarPrestamosVencidosAsync();
        return Ok(new { message = "Notificaciones de préstamos vencidos enviadas." });
    }

    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] UpdateNotificacionDto dto)
    {
        var result = await _notificacionService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _notificacionService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
