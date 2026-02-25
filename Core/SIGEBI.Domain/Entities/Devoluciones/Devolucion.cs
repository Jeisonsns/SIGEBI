using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Devoluciones;

public class Devolucion : AuditEntity
{
    public string PrestamoId { get; set; } = string.Empty;
    public DateTime FechaDevolucion { get; set; }
    public bool EsTardia { get; set; }
}
