namespace SIGEBI.Domain.Base;

public abstract class AuditEntity
{
    public string Id { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaModificacion { get; set; }
}
