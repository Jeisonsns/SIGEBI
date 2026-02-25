using SIGEBI.Domain.Entities.Auditoria;

namespace SIGEBI.Domain.Repository;

public interface IAuditoriaRepository : IBaseRepository<Auditoria>
{
    Task<IEnumerable<Auditoria>> GetByFechaAsync(DateTime desde, DateTime hasta);
    Task<IEnumerable<Auditoria>> GetByUsuarioAsync(string usuario);
}
