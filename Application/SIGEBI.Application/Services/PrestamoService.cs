using SIGEBI.Application.Dtos.Prestamos;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Prestamos;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class PrestamoService : IPrestamoService
{
    private readonly IPrestamoRepository _prestamoRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IRecursoRepository _recursoRepo;
    private readonly IPenalizacionRepository _penalizacionRepo;

    public PrestamoService(IPrestamoRepository prestamoRepo, IUsuarioRepository usuarioRepo,
        IRecursoRepository recursoRepo, IPenalizacionRepository penalizacionRepo)
    {
        _prestamoRepo = prestamoRepo;
        _usuarioRepo = usuarioRepo;
        _recursoRepo = recursoRepo;
        _penalizacionRepo = penalizacionRepo;
    }

    public async Task<IEnumerable<PrestamoDto>> GetAllAsync()
        => (await _prestamoRepo.GetAllAsync()).Select(ToDto);

    public async Task<PrestamoDto?> GetByIdAsync(string id)
    {
        var p = await _prestamoRepo.GetByIdAsync(id);
        return p == null ? null : ToDto(p);
    }

    public async Task<IEnumerable<PrestamoDto>> GetActivosByUsuarioAsync(string usuarioId)
        => (await _prestamoRepo.GetActivosByUsuarioAsync(usuarioId)).Select(ToDto);

    public async Task<IEnumerable<PrestamoDto>> GetVencidosAsync()
        => (await _prestamoRepo.GetVencidosAsync()).Select(ToDto);

    public async Task<OperationResult> SaveAsync(SavePrestamoDto dto)
    {
        try
        {
            var usuario = await _usuarioRepo.GetByIdAsync(dto.UsuarioId);
            if (usuario == null || !usuario.EstaActivo())
                return OperationResult.Fail("Usuario no autorizado o inactivo.");

            var penalizaciones = await _penalizacionRepo.GetActivasByUsuarioAsync(dto.UsuarioId);
if (penalizaciones.Any())
    return OperationResult.Fail("El usuario tiene penalizaciones activas.");

var vencidos = await _prestamoRepo.GetVencidosAsync();
if (vencidos.Any(p => p.UsuarioId == dto.UsuarioId))
    return OperationResult.Fail("El usuario tiene préstamos vencidos.");

var recurso = await _recursoRepo.GetByIdAsync(dto.RecursoId);
if (recurso == null || !recurso.EstaDisponible())
    return OperationResult.Fail("El recurso no está disponible.");

var activos = await _prestamoRepo.GetActivosByUsuarioAsync(dto.UsuarioId);
if (activos.Count() >= usuario.LimitePrestamos())
    return OperationResult.Fail("El usuario alcanzó el límite de préstamos simultáneos.");

var prestamo = Prestamo.Crear(dto.UsuarioId, dto.RecursoId, usuario.DiasPrestamoPermitidos());
await _prestamoRepo.AddAsync(prestamo);
recurso.MarcarComoPrestado();
await _recursoRepo.UpdateAsync(recurso);
return OperationResult.Ok("Préstamo autorizado correctamente.");
        }
        catch (InvalidOperationException ex) { return OperationResult.Fail(ex.Message); }
        catch (Exception) { return OperationResult.Fail("Error inesperado al procesar el préstamo."); }
    }

    public async Task<OperationResult> UpdateAsync(UpdatePrestamoDto dto)
{
    try
    {
        var prestamo = await _prestamoRepo.GetByIdAsync(dto.Id);
        if (prestamo == null) return OperationResult.Fail("Préstamo no encontrado.");
        await _prestamoRepo.UpdateAsync(prestamo);
        return OperationResult.Ok("Préstamo actualizado.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al actualizar el préstamo."); }
}

public async Task<OperationResult> RenovarAsync(string prestamoId)
{
    try
    {
        var prestamo = await _prestamoRepo.GetByIdAsync(prestamoId);
        if (prestamo == null) return OperationResult.Fail("Préstamo no encontrado.");
        var usuario = await _usuarioRepo.GetByIdAsync(prestamo.UsuarioId);
        prestamo.Renovar(usuario?.DiasPrestamoPermitidos() ?? 7);
        await _prestamoRepo.UpdateAsync(prestamo);
        return OperationResult.Ok("Préstamo renovado correctamente.");
    }
    catch (InvalidOperationException ex) { return OperationResult.Fail(ex.Message); }
    catch (Exception) { return OperationResult.Fail("Error inesperado al renovar el préstamo."); }
}

public async Task<OperationResult> DeleteAsync(string id)
{
    try
    {
        var prestamo = await _prestamoRepo.GetByIdAsync(id);
        if (prestamo == null) return OperationResult.Fail("Préstamo no encontrado.");
        await _prestamoRepo.DeleteAsync(id);
        return OperationResult.Ok("Préstamo eliminado.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al eliminar el préstamo."); }
}

private static PrestamoDto ToDto(Prestamo p) => new()
{
    Id = p.Id,
    UsuarioId = p.UsuarioId,
    RecursoId = p.RecursoId,
    FechaInicio = p.FechaInicio,
    FechaLimite = p.FechaLimite,
    Estado = p.Estado
};
}
