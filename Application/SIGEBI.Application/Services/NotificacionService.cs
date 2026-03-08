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
    private readonly IUsuarioRepository _usuarioRepo;

    public NotificacionService(INotificacionRepository notificacionRepo,
        IPrestamoRepository prestamoRepo, IUsuarioRepository usuarioRepo)
    {
        _notificacionRepo = notificacionRepo;
        _prestamoRepo = prestamoRepo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<IEnumerable<NotificacionDto>> GetAllAsync()
    {
        var notificaciones = await _notificacionRepo.GetAllAsync();
        return notificaciones.Select(n => ToDto(n));
    }

    public async Task<NotificacionDto?> GetByIdAsync(string id)
    {
        var n = await _notificacionRepo.GetByIdAsync(id);
        return n == null ? null : ToDto(n);
    }

    public async Task<IEnumerable<NotificacionDto>> GetByUsuarioAsync(string usuarioId)
    {
        var notificaciones = await _notificacionRepo.GetByUsuarioAsync(usuarioId);
        return notificaciones.Select(n => ToDto(n));
    }

    public async Task<OperationResult> EnviarAsync(SaveNotificacionDto dto)
    {
        var notificacion = new Notificacion
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = dto.UsuarioId, Tipo = dto.Tipo,
            Asunto = dto.Asunto, Mensaje = dto.Mensaje,
            FechaEnvio = DateTime.UtcNow, Estado = EstadoNotificacion.Enviada
        };
        await _notificacionRepo.AddAsync(notificacion);
        return OperationResult.Ok("Notificación enviada.");
    }

    public async Task NotificarVencimientosProximosAsync()
    {
        var prestamos = await _prestamoRepo.GetAllAsync();
        var proximos = prestamos.Where(p =>
            p.Estado == Domain.Entities.Prestamos.EstadoPrestamo.Activo &&
            p.FechaLimite > DateTime.UtcNow &&
            p.FechaLimite <= DateTime.UtcNow.AddDays(2));

        foreach (var prestamo in proximos)
        {
            await EnviarAsync(new SaveNotificacionDto
            {
                UsuarioId = prestamo.UsuarioId,
                Tipo = TipoNotificacion.VencimientoProximo,
                Asunto = "Vencimiento próximo de préstamo",
                Mensaje = $"Tu préstamo vence el {prestamo.FechaLimite:dd/MM/yyyy}. Por favor devuelve el recurso a tiempo."
            });
        }
    }

    public async Task NotificarPrestamosVencidosAsync()
    {
        var vencidos = await _prestamoRepo.GetVencidosAsync();
        foreach (var prestamo in vencidos)
        {
            await EnviarAsync(new SaveNotificacionDto
            {
                UsuarioId = prestamo.UsuarioId,
                Tipo = TipoNotificacion.PrestamoVencido,
                Asunto = "Préstamo vencido",
                Mensaje = $"Tu préstamo venció el {prestamo.FechaLimite:dd/MM/yyyy}. Devuelve el recurso a la brevedad posible."
            });
        }
    }

    public async Task<OperationResult> SaveAsync(SaveNotificacionDto dto) => await EnviarAsync(dto);

    public async Task<OperationResult> UpdateAsync(UpdateNotificacionDto dto)
    {
        var n = await _notificacionRepo.GetByIdAsync(dto.Id);
        if (n == null) return OperationResult.Fail("Notificación no encontrada.");
        n.Estado = dto.Estado;
        await _notificacionRepo.UpdateAsync(n);
        return OperationResult.Ok("Notificación actualizada.");
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var n = await _notificacionRepo.GetByIdAsync(id);
        if (n == null) return OperationResult.Fail("Notificación no encontrada.");
        await _notificacionRepo.DeleteAsync(id);
        return OperationResult.Ok("Notificación eliminada.");
    }

    private static NotificacionDto ToDto(Notificacion n) => new()
    {
        Id = n.Id, UsuarioId = n.UsuarioId, Tipo = n.Tipo,
        Asunto = n.Asunto, Mensaje = n.Mensaje,
        FechaEnvio = n.FechaEnvio, Estado = n.Estado
    };
}
