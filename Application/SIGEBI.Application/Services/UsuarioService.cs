using SIGEBI.Application.Dtos.Usuarios;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Usuarios;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IPrestamoRepository _prestamoRepo;
    private readonly IPenalizacionRepository _penalizacionRepo;

    public UsuarioService(IUsuarioRepository usuarioRepo, IPrestamoRepository prestamoRepo, IPenalizacionRepository penalizacionRepo)
    {
        _usuarioRepo = usuarioRepo;
        _prestamoRepo = prestamoRepo;
        _penalizacionRepo = penalizacionRepo;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var usuarios = await _usuarioRepo.GetAllAsync();
        return usuarios.Select(u => ToDto(u));
    }

    public async Task<UsuarioDto?> GetByIdAsync(string id)
    {
        var u = await _usuarioRepo.GetByIdAsync(id);
        return u == null ? null : ToDto(u);
    }

    public async Task<UsuarioDto?> GetByCodigoAsync(string codigo)
    {
        var u = await _usuarioRepo.GetByCodigoAsync(codigo);
        return u == null ? null : ToDto(u);
    }

    public async Task<bool> TieneCondicionesDeAccesoAsync(string usuarioId)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(usuarioId);
        if (usuario == null || usuario.Estado == EstadoUsuario.Inactivo) return false;

        var prestamosVencidos = await _prestamoRepo.GetVencidosAsync();
        if (prestamosVencidos.Any(p => p.UsuarioId == usuarioId)) return false;

        var penalizaciones = await _penalizacionRepo.GetActivasByUsuarioAsync(usuarioId);
        if (penalizaciones.Any()) return false;

        return true;
    }

    public async Task<OperationResult> SaveAsync(SaveUsuarioDto dto)
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid().ToString(),
            Codigo = dto.Codigo, Nombre = dto.Nombre,
            Email = dto.Email, Tipo = dto.Tipo,
            Estado = EstadoUsuario.Activo,
            FechaRegistro = DateTime.UtcNow
        };
        await _usuarioRepo.AddAsync(usuario);
        return OperationResult.Ok("Usuario registrado correctamente.");
    }

    public async Task<OperationResult> UpdateAsync(UpdateUsuarioDto dto)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(dto.Id);
        if (usuario == null) return OperationResult.Fail("Usuario no encontrado.");
        usuario.Nombre = dto.Nombre;
        usuario.Email = dto.Email;
        usuario.Estado = dto.Estado;
        await _usuarioRepo.UpdateAsync(usuario);
        return OperationResult.Ok("Usuario actualizado.");
    }

    public async Task<OperationResult> CambiarEstadoAsync(string id, EstadoUsuario estado)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null) return OperationResult.Fail("Usuario no encontrado.");
        usuario.Estado = estado;
        await _usuarioRepo.UpdateAsync(usuario);
        return OperationResult.Ok("Estado actualizado.");
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null) return OperationResult.Fail("Usuario no encontrado.");
        await _usuarioRepo.DeleteAsync(id);
        return OperationResult.Ok("Usuario eliminado.");
    }

    private static UsuarioDto ToDto(Usuario u) => new()
    {
        Id = u.Id, Codigo = u.Codigo, Nombre = u.Nombre,
        Email = u.Email, Tipo = u.Tipo, Estado = u.Estado,
        FechaRegistro = u.FechaRegistro
    };
}
