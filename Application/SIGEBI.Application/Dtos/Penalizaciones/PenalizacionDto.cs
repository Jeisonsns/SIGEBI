using SIGEBI.Application.Base;
using SIGEBI.Domain.Entities.Penalizaciones;

namespace SIGEBI.Application.Dtos.Penalizaciones;

public class PenalizacionDto : DtoBase
{
    public string UsuarioId { get; set; } = string.Empty;
    public string Causa { get; set; } = string.Empty;
    public TipoPenalizacion Tipo { get; set; }
    public EstadoPenalizacion Estado { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
