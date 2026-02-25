using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Recursos;

public class Recurso : AuditEntity
{
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Editorial { get; set; } = string.Empty;
    public int Anio { get; set; }
    public int NumEjemplares { get; set; }
    public EstadoRecurso Estado { get; set; }
}
