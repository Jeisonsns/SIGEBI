using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Notificaciones;

public class Notificacion : AuditEntity
{
    public string UsuarioId { get; private set; } = string.Empty;
    public TipoNotificacion Tipo { get; private set; }
    public string Asunto { get; private set; } = string.Empty;
    public string Mensaje { get; private set; } = string.Empty;
    public DateTime FechaEnvio { get; private set; }
    public EstadoNotificacion Estado { get; private set; }

    private Notificacion() { }

    public static Notificacion Crear(string usuarioId, TipoNotificacion tipo, string asunto, string mensaje)
    {
        if (string.IsNullOrWhiteSpace(usuarioId)) throw new ArgumentException("El usuarioId es requerido.");
        if (string.IsNullOrWhiteSpace(asunto)) throw new ArgumentException("El asunto es requerido.");
        if (string.IsNullOrWhiteSpace(mensaje)) throw new ArgumentException("El mensaje es requerido.");

        return new Notificacion
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = usuarioId,
            Tipo = tipo,
            Asunto = asunto,
            Mensaje = mensaje,
            FechaEnvio = DateTime.UtcNow,
            Estado = EstadoNotificacion.Enviada,
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow
        };
    }

    public void MarcarComoFallida()
    {
        Estado = EstadoNotificacion.Fallida;
        FechaModificacion = DateTime.UtcNow;
    }
}