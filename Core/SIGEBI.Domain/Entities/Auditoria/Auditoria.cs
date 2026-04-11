using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Auditoria;

public class Auditoria : AuditEntity
{
    public string Operacion { get; private set; } = string.Empty;
    public string Usuario { get; private set; } = string.Empty;
    public string Entidad { get; private set; } = string.Empty;
    public string RecursoId { get; private set; } = string.Empty;
    public DateTime Fecha { get; private set; }
    public string Detalles { get; private set; } = string.Empty;
    public string Resultado { get; private set; } = string.Empty;

    private Auditoria() { }

    public static Auditoria Registrar(string operacion, string usuario,
        string entidad, string recursoId, string detalles, string resultado)
    {
        return new Auditoria
        {
            Id = Guid.NewGuid().ToString(),
            Operacion = operacion,
            Usuario = usuario,
            Entidad = entidad,
            RecursoId = recursoId,
            Fecha = DateTime.UtcNow,
            Detalles = detalles,
            Resultado = resultado,
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow
        };
    }
}