using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Usuarios;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Entities.Usuarios;

namespace SIGEBI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioService.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null) return NotFound(new { message = "Usuario no encontrado." });
        return Ok(usuario);
    }

    [HttpGet("codigo/{codigo}")]
    public async Task<IActionResult> GetByCodigo(string codigo)
    {
        var usuario = await _usuarioService.GetByCodigoAsync(codigo);
        if (usuario == null) return NotFound(new { message = "Usuario no encontrado." });
        return Ok(usuario);
    }

    [HttpGet("{id}/acceso")]
    public async Task<IActionResult> VerificarAcceso(string id)
    {
        var tieneAcceso = await _usuarioService.TieneCondicionesDeAccesoAsync(id);
        return Ok(new { tieneAcceso });
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SaveUsuarioDto dto)
    {
        var result = await _usuarioService.SaveAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUsuarioDto dto)
    {
        var result = await _usuarioService.UpdateAsync(dto);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(string id, [FromBody] EstadoUsuario estado)
    {
        var result = await _usuarioService.CambiarEstadoAsync(id, estado);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _usuarioService.DeleteAsync(id);
        if (!result.Success) return BadRequest(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}
