namespace SIGEBI.Application.Dtos.Auditoria;

public class SaveAuditoriaDto
{
    public string Operacion { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public string RecursoId { get; set; } = string.Empty;
    public string Detalles { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
}
