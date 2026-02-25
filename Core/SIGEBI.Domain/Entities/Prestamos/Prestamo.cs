using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Prestamos;

public class Prestamo : AuditEntity
{
    public string UsuarioId { get; set; } = string.Empty;
    public string RecursoId { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaLimite { get; set; }
    public EstadoPrestamo Estado { get; set; }
}
