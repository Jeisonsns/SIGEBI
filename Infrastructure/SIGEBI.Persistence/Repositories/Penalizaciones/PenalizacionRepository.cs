using MongoDB.Driver;
using SIGEBI.Domain.Entities.Penalizaciones;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Penalizaciones;

public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
{
    public PenalizacionRepository(IMongoDatabase database) : base(database, "penalizaciones") { }

    public async Task<IEnumerable<Penalizacion>> GetActivasByUsuarioAsync(string usuarioId)
    {
        try
        {
            var ahora = DateTime.UtcNow;
            return await _collection
                .Find(p => p.UsuarioId == usuarioId &&
                           p.Estado == EstadoPenalizacion.Activa &&
                           p.FechaFin > ahora)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al obtener penalizaciones activas del usuario.", ex);
        }
    }
}
