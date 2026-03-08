using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Notificaciones;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces;

public interface INotificacionService : IBaseService<NotificacionDto, SaveNotificacionDto, UpdateNotificacionDto>
{
    Task<IEnumerable<NotificacionDto>> GetByUsuarioAsync(string usuarioId);
    Task<OperationResult> EnviarAsync(SaveNotificacionDto dto);
    Task NotificarVencimientosProximosAsync();
    Task NotificarPrestamosVencidosAsync();
}
