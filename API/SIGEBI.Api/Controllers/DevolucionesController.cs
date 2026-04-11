using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Devoluciones;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DevolucionesController : ControllerBase
{
    private readonly IDevolucionService _devolucionService;

    public DevolucionesController(IDevolucionService devolucionService)
    {
        _devolucionService = devolucionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DevolucionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DevolucionDto>>> GetAll()
        => Ok(await _devolucionService.GetAllAsync());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DevolucionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DevolucionDto>> GetById(string id)
    {
        var devolucion = await _devolucionService.GetByIdAsync(id);
        if (devolucion == null) return NotFound(new { message = "Devolución no encontrada." });
        return Ok(devolucion);
    }

    [HttpPost("procesar/{prestamoId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Procesar(string prestamoId)
    {
        var result = await _devolucionService.ProcesarDevolucionAsync(prestamoId);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return StatusCode(StatusCodes.Status201Created, new { message = result.Message });
    }

    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] UpdateDevolucionDto dto)
    {
        var result = await _devolucionService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _devolucionService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
