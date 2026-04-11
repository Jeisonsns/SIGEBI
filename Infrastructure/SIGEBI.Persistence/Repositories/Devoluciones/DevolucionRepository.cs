using MongoDB.Driver;
using SIGEBI.Domain.Entities.Devoluciones;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Devoluciones;

public class DevolucionRepository : BaseRepository<Devolucion>, IDevolucionRepository
{
    public DevolucionRepository(IMongoDatabase database) : base(database, "devoluciones") { }

    public async Task<Devolucion?> GetByPrestamoAsync(string prestamoId)
    {
        try
        {
            return await _collection.Find(d => d.PrestamoId == prestamoId).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al buscar devolución por préstamo.", ex);
        }
    }
}
