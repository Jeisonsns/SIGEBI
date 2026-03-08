using SIGEBI.Domain.Entities.Usuarios;

namespace SIGEBI.Application.Dtos.Usuarios;

public class UpdateUsuarioDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public EstadoUsuario Estado { get; set; }
}
