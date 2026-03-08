using SIGEBI.Domain.Entities.Prestamos;

namespace SIGEBI.Application.Dtos.Prestamos;

public class UpdatePrestamoDto
{
    public string Id { get; set; } = string.Empty;
    public EstadoPrestamo Estado { get; set; }
}
