using SIGEBI.Application.Base;

namespace SIGEBI.Application.Dtos.Devoluciones;

public class DevolucionDto : DtoBase
{
    public string PrestamoId { get; set; } = string.Empty;
    public DateTime FechaDevolucion { get; set; }
    public bool EsTardia { get; set; }
}
