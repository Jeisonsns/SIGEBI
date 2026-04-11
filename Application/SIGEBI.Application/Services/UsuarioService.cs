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

    public UsuarioService(IUsuarioRepository usuarioRepo, IPrestamoRepository prestamoRepo,
        IPenalizacionRepository penalizacionRepo)
    {
        _usuarioRepo = usuarioRepo;
        _prestamoRepo = prestamoRepo;
        _penalizacionRepo = penalizacionRepo;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var usuarios = await _usuarioRepo.GetAllAsync();
        return usuarios.Select(ToDto);
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
        try
        {
            var usuario = await _usuarioRepo.GetByIdAsync(usuarioId);
            if (usuario == null || !usuario.EstaActivo()) return false;
            var penalizaciones = await _penalizacionRepo.GetActivasByUsuarioAsync(usuarioId);
            if (penalizaciones.Any()) return false;
            var vencidos = await _prestamoRepo.GetVencidosAsync();
            if (vencidos.Any(p => p.UsuarioId == usuarioId)) return false;
            return true;
        }
        catch { return false; }
    }

    public async Task<OperationResult> SaveAsync(SaveUsuarioDto dto)
    {
        try
        {
            var usuario = Usuario.Crear(dto.Codigo, dto.Nombre, dto.Email, dto.Tipo);
            await _usuarioRepo.AddAsync(usuario);
            return OperationResult.Ok("Usuario registrado correctamente.");
        }
        catch (ArgumentException ex) { return OperationResult.Fail(ex.Message); }
        catch (Exception) { return OperationResult.Fail("Error inesperado al registrar el usuario."); }
    }

    public async Task<OperationResult> UpdateAsync(UpdateUsuarioDto dto)
{
    try
    {
        var usuario = await _usuarioRepo.GetByIdAsync(dto.Id);
        if (usuario == null) return OperationResult.Fail("Usuario no encontrado.");
        usuario.Actualizar(dto.Nombre, dto.Email);
        await _usuarioRepo.UpdateAsync(usuario);
        return OperationResult.Ok("Usuario actualizado.");
    }
    catch (ArgumentException ex) { return OperationResult.Fail(ex.Message); }
    catch (Exception) { return OperationResult.Fail("Error inesperado al actualizar el usuario."); }
}

public async Task<OperationResult> CambiarEstadoAsync(string id, EstadoUsuario estado)
{
    try
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null) return OperationResult.Fail("Usuario no encontrado.");
        if (estado == EstadoUsuario.Activo) usuario.Activar();
        else usuario.Desactivar();
        await _usuarioRepo.UpdateAsync(usuario);
        return OperationResult.Ok("Estado actualizado.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al cambiar el estado."); }
}

public async Task<OperationResult> DeleteAsync(string id)
{
    try
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null) return OperationResult.Fail("Usuario no encontrado.");
        await _usuarioRepo.DeleteAsync(id);
        return OperationResult.Ok("Usuario eliminado.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al eliminar el usuario."); }
}

private static UsuarioDto ToDto(Usuario u) => new()
{
    Id = u.Id,
    Codigo = u.Codigo,
    Nombre = u.Nombre,
    Email = u.Email,
    Tipo = u.Tipo,
    Estado = u.Estado,
    FechaRegistro = u.FechaRegistro
};
}
