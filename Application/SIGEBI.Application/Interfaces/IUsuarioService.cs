using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Usuarios;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Usuarios;

namespace SIGEBI.Application.Interfaces;

public interface IUsuarioService : IBaseService<UsuarioDto, SaveUsuarioDto, UpdateUsuarioDto>
{
    Task<UsuarioDto?> GetByCodigoAsync(string codigo);
    Task<OperationResult> CambiarEstadoAsync(string id, EstadoUsuario estado);
    Task<bool> TieneCondicionesDeAccesoAsync(string usuarioId);
}
