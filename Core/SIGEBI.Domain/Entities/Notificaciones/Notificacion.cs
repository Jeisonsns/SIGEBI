using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Notificaciones;

public class Notificacion : AuditEntity
{
    public string UsuarioId { get; set; } = string.Empty;
    public TipoNotificacion Tipo { get; set; }
    public string Asunto { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaEnvio { get; set; }
    public EstadoNotificacion Estado { get; set; }
}
