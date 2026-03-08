using SIGEBI.Domain.Entities.Notificaciones;

namespace SIGEBI.Application.Dtos.Notificaciones;

public class UpdateNotificacionDto
{
    public string Id { get; set; } = string.Empty;
    public EstadoNotificacion Estado { get; set; }
}
