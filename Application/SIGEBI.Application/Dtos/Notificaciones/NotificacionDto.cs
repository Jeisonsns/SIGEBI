using SIGEBI.Application.Base;
using SIGEBI.Domain.Entities.Notificaciones;

namespace SIGEBI.Application.Dtos.Notificaciones;

public class NotificacionDto : DtoBase
{
    public string UsuarioId { get; set; } = string.Empty;
    public TipoNotificacion Tipo { get; set; }
    public string Asunto { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public EstadoNotificacion Estado { get; set; }
}
