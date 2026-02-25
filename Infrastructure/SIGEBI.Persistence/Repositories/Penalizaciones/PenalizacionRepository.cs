using MongoDB.Driver;
using SIGEBI.Domain.Entities.Penalizaciones;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Penalizaciones;

public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
{
    public PenalizacionRepository(IMongoDatabase database) : base(database, "penalizaciones") { }

    public async Task<IEnumerable<Penalizacion>> GetActivasByUsuarioAsync(string usuarioId)
        => await _collection.Find(p => p.UsuarioId == usuarioId && p.Estado == EstadoPenalizacion.Activa).ToListAsync();
}
