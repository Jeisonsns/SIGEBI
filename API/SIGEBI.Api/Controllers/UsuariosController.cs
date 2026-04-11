using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Usuarios;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities.Usuarios;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        => Ok(await _usuarioService.GetAllAsync());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetById(string id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null) return NotFound(new { message = "Usuario no encontrado." });
        return Ok(usuario);
    }

    [HttpGet("codigo/{codigo}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetByCodigo(string codigo)
    {
        var usuario = await _usuarioService.GetByCodigoAsync(codigo);
        if (usuario == null) return NotFound(new { message = "Usuario no encontrado." });
        return Ok(usuario);
    }

    [HttpGet("{id}/acceso")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult> VerificarAcceso(string id)
    {
        var tieneAcceso = await _usuarioService.TieneCondicionesDeAccesoAsync(id);
        return Ok(new { tieneAcceso });
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Save([FromBody] SaveUsuarioDto dto)
    {
        var result = await _usuarioService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return StatusCode(StatusCodes.Status201Created, new { message = result.Message });
    }

    [HttpPut]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] UpdateUsuarioDto dto)
    {
        var result = await _usuarioService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id}/estado")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CambiarEstado(string id, [FromBody] EstadoUsuario estado)
    {
        var result = await _usuarioService.CambiarEstadoAsync(id, estado);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _usuarioService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
