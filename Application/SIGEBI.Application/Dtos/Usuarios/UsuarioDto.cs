using SIGEBI.Application.Base;
using SIGEBI.Domain.Entities.Usuarios;

namespace SIGEBI.Application.Dtos.Usuarios;

public class UsuarioDto : DtoBase
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public TipoUsuario Tipo { get; set; }
    public EstadoUsuario Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
}
