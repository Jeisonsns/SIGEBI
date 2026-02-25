using MongoDB.Driver;
using SIGEBI.Domain.Entities.Recursos;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Recursos;

public class RecursoRepository : BaseRepository<Recurso>, IRecursoRepository
{
    public RecursoRepository(IMongoDatabase database) : base(database, "recursos") { }

    public async Task<IEnumerable<Recurso>> GetByEstadoAsync(EstadoRecurso estado)
        => await _collection.Find(r => r.Estado == estado).ToListAsync();
}
