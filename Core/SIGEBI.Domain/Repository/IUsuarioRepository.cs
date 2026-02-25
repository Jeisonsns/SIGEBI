using SIGEBI.Domain.Entities.Usuarios;

namespace SIGEBI.Domain.Repository;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario?> GetByCodigoAsync(string codigo);
}
