using SIGEBI.Domain.Entities.Penalizaciones;

namespace SIGEBI.Domain.Repository;

public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
{
    Task<IEnumerable<Penalizacion>> GetActivasByUsuarioAsync(string usuarioId);
}
