using MongoDB.Driver;
using AuditoriaEntity = SIGEBI.Domain.Entities.Auditoria.Auditoria;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Auditoria;

public class AuditoriaRepository : BaseRepository<AuditoriaEntity>, IAuditoriaRepository
{
    public AuditoriaRepository(IMongoDatabase database) : base(database, "auditoria") { }

    public async Task<IEnumerable<AuditoriaEntity>> GetByFechaAsync(DateTime desde, DateTime hasta)
        => await _collection.Find(a => a.Fecha >= desde && a.Fecha <= hasta).ToListAsync();

    public async Task<IEnumerable<AuditoriaEntity>> GetByUsuarioAsync(string usuario)
        => await _collection.Find(a => a.Usuario == usuario).ToListAsync();
}