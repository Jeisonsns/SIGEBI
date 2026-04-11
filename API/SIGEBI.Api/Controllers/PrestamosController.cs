using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Prestamos;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoService _prestamoService;

    public PrestamosController(IPrestamoService prestamoService)
    {
        _prestamoService = prestamoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PrestamoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrestamoDto>>> GetAll()
        => Ok(await _prestamoService.GetAllAsync());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PrestamoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrestamoDto>> GetById(string id)
    {
        var prestamo = await _prestamoService.GetByIdAsync(id);
        if (prestamo == null) return NotFound(new { message = "Préstamo no encontrado." });
        return Ok(prestamo);
    }

    [HttpGet("activos/{usuarioId}")]
    [ProducesResponseType(typeof(IEnumerable<PrestamoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrestamoDto>>> GetActivosByUsuario(string usuarioId)
        => Ok(await _prestamoService.GetActivosByUsuarioAsync(usuarioId));

    [HttpGet("vencidos")]
    [ProducesResponseType(typeof(IEnumerable<PrestamoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrestamoDto>>> GetVencidos()
        => Ok(await _prestamoService.GetVencidosAsync());

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Save([FromBody] SavePrestamoDto dto)
    {
        var result = await _prestamoService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return StatusCode(StatusCodes.Status201Created, new { message = result.Message });
    }

    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] UpdatePrestamoDto dto)
    {
        var result = await _prestamoService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id}/renovar")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Renovar(string id)
    {
        var result = await _prestamoService.RenovarAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _prestamoService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
