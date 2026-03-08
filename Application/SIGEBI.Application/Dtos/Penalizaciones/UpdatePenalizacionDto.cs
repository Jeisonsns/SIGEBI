using SIGEBI.Domain.Entities.Penalizaciones;

namespace SIGEBI.Application.Dtos.Penalizaciones;

public class UpdatePenalizacionDto
{
    public string Id { get; set; } = string.Empty;
    public EstadoPenalizacion Estado { get; set; }
}
