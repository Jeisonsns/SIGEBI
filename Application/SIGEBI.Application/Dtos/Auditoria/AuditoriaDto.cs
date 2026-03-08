using SIGEBI.Application.Base;

namespace SIGEBI.Application.Dtos.Auditoria;

public class AuditoriaDto : DtoBase
{
    public string Operacion { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public string RecursoId { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Detalles { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
}
