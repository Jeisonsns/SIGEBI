using SIGEBI.Domain.Entities.Penalizaciones;

namespace SIGEBI.Application.Dtos.Penalizaciones;

public class SavePenalizacionDto
{
    public string UsuarioId { get; set; } = string.Empty;
    public string Causa { get; set; } = string.Empty;
    public TipoPenalizacion Tipo { get; set; }
    public DateTime FechaFin { get; set; }
}
