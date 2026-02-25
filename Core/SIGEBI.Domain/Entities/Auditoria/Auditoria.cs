using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Auditoria;

public class Auditoria : AuditEntity
{
    public string Operacion { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public string RecursoId { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Detalles { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
}
