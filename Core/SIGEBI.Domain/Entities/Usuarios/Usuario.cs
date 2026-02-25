using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Usuarios;

public class Usuario : AuditEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public TipoUsuario Tipo { get; set; }
    public EstadoUsuario Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
}
