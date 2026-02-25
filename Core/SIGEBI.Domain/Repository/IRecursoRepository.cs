using SIGEBI.Domain.Entities.Recursos;

namespace SIGEBI.Domain.Repository;

public interface IRecursoRepository : IBaseRepository<Recurso>
{
    Task<IEnumerable<Recurso>> GetByEstadoAsync(EstadoRecurso estado);
}
