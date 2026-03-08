using SIGEBI.Domain.Entities.Usuarios;

namespace SIGEBI.Application.Dtos.Usuarios;

public class SaveUsuarioDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public TipoUsuario Tipo { get; set; }
}
