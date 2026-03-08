using SIGEBI.Application.Base;
using SIGEBI.Domain.Entities.Prestamos;

namespace SIGEBI.Application.Dtos.Prestamos;

public class PrestamoDto : DtoBase
{
    public string UsuarioId { get; set; } = string.Empty;
    public string RecursoId { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaLimite { get; set; }
    public EstadoPrestamo Estado { get; set; }
}
