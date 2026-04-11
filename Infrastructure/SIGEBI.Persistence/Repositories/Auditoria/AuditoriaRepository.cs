using MongoDB.Driver;
using AuditoriaEntity = SIGEBI.Domain.Entities.Auditoria.Auditoria;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Auditoria;

public class AuditoriaRepository : BaseRepository<AuditoriaEntity>, IAuditoriaRepository
{
    public AuditoriaRepository(IMongoDatabase database) : base(database, "auditoria") { }

    public async Task<IEnumerable<AuditoriaEntity>> GetByFechaAsync(DateTime desde, DateTime hasta)
    {
        try
        {
            return await _collection
                .Find(a => a.Fecha >= desde && a.Fecha <= hasta)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al filtrar auditoría por fecha.", ex);
        }
    }

    public async Task<IEnumerable<AuditoriaEntity>> GetByUsuarioAsync(string usuario)
    {
        try
        {
            return await _collection.Find(a => a.Usuario == usuario).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al filtrar auditoría por usuario.", ex);
        }
    }
}
