using SIGEBI.Application.Dtos.Notificaciones;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Notificaciones;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class NotificacionService : INotificacionService
{
    private readonly INotificacionRepository _notificacionRepo;
    private readonly IPrestamoRepository _prestamoRepo;

    public NotificacionService(INotificacionRepository notificacionRepo, IPrestamoRepository prestamoRepo)
    {
        _notificacionRepo = notificacionRepo;
        _prestamoRepo = prestamoRepo;
    }

    public async Task<IEnumerable<NotificacionDto>> GetAllAsync()
        => (await _notificacionRepo.GetAllAsync()).Select(ToDto);

    public async Task<NotificacionDto?> GetByIdAsync(string id)
    {
        var n = await _notificacionRepo.GetByIdAsync(id);
        return n == null ? null : ToDto(n);
    }

    public async Task<IEnumerable<NotificacionDto>> GetByUsuarioAsync(string usuarioId)
        => (await _notificacionRepo.GetByUsuarioAsync(usuarioId)).Select(ToDto);

    public async Task<OperationResult> EnviarAsync(SaveNotificacionDto dto)
    {
        try
        {
            var notificacion = Notificacion.Crear(dto.UsuarioId, dto.Tipo, dto.Asunto, dto.Mensaje);
            await _notificacionRepo.AddAsync(notificacion);
            return OperationResult.Ok("Notificación enviada.");
        }
        catch (ArgumentException ex) { return OperationResult.Fail(ex.Message); }
        catch (Exception) { return OperationResult.Fail("Error inesperado al enviar la notificación."); }
    }

    public async Task NotificarVencimientosProximosAsync()
{
    var prestamos = await _prestamoRepo.GetAllAsync();
    var proximos = prestamos.Where(p =>
        p.EstaActivo() &&
        p.FechaLimite > DateTime.UtcNow &&
        p.FechaLimite <= DateTime.UtcNow.AddDays(2));

    foreach (var prestamo in proximos)
    {
        try
        {
            await EnviarAsync(new SaveNotificacionDto
            {
                UsuarioId = prestamo.UsuarioId,
                Tipo = TipoNotificacion.VencimientoProximo,
                Asunto = "Vencimiento próximo de préstamo",
                Mensaje = $"Tu préstamo vence el {prestamo.FechaLimite:dd/MM/yyyy}. Por favor devuelve el recurso a tiempo."
            });
        }
        catch { /* continuar con los demás aunque uno falle */ }
    }
}

public async Task NotificarPrestamosVencidosAsync()
{
    var vencidos = await _prestamoRepo.GetVencidosAsync();
    foreach (var prestamo in vencidos)
    {
        try
        {
            await EnviarAsync(new SaveNotificacionDto
            {
                UsuarioId = prestamo.UsuarioId,
                Tipo = TipoNotificacion.PrestamoVencido,
                Asunto = "Préstamo vencido",
                Mensaje = $"Tu préstamo venció el {prestamo.FechaLimite:dd/MM/yyyy}. Devuelve el recurso a la brevedad."
            });
        }
        catch { /* continuar con los demás aunque uno falle */ }
    }
}

public async Task<OperationResult> SaveAsync(SaveNotificacionDto dto) => await EnviarAsync(dto);

public async Task<OperationResult> UpdateAsync(UpdateNotificacionDto dto)
{
    try
    {
        var n = await _notificacionRepo.GetByIdAsync(dto.Id);
        if (n == null) return OperationResult.Fail("Notificación no encontrada.");
        await _notificacionRepo.UpdateAsync(n);
        return OperationResult.Ok("Notificación actualizada.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al actualizar la notificación."); }
}

public async Task<OperationResult> DeleteAsync(string id)
{
    try
    {
        var n = await _notificacionRepo.GetByIdAsync(id);
        if (n == null) return OperationResult.Fail("Notificación no encontrada.");
        await _notificacionRepo.DeleteAsync(id);
        return OperationResult.Ok("Notificación eliminada.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al eliminar la notificación."); }
}

private static NotificacionDto ToDto(Notificacion n) => new()
{
    Id = n.Id,
    UsuarioId = n.UsuarioId,
    Tipo = n.Tipo,
    Asunto = n.Asunto,
    Mensaje = n.Mensaje,
    FechaEnvio = n.FechaEnvio,
    Estado = n.Estado
};
}
