using MongoDB.Driver;
using SIGEBI.Domain.Entities.Devoluciones;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Devoluciones;

public class DevolucionRepository : BaseRepository<Devolucion>, IDevolucionRepository
{
    public DevolucionRepository(IMongoDatabase database) : base(database, "devoluciones") { }

    public async Task<Devolucion?> GetByPrestamoAsync(string prestamoId)
        => await _collection.Find(d => d.PrestamoId == prestamoId).FirstOrDefaultAsync();
}
