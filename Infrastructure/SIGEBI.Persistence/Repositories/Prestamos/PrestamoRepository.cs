using MongoDB.Driver;
using SIGEBI.Domain.Entities.Prestamos;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Prestamos;

public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
{
    public PrestamoRepository(IMongoDatabase database) : base(database, "prestamos") { }

    public async Task<IEnumerable<Prestamo>> GetActivosByUsuarioAsync(string usuarioId)
    {
        try
        {
            return await _collection
                .Find(p => p.UsuarioId == usuarioId && p.Estado == EstadoPrestamo.Activo)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al obtener préstamos activos del usuario.", ex);
        }
    }

    public async Task<IEnumerable<Prestamo>> GetVencidosAsync()
    {
        try
        {
            var ahora = DateTime.UtcNow;
            return await _collection
                .Find(p => p.Estado == EstadoPrestamo.Activo && p.FechaLimite < ahora)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al obtener préstamos vencidos.", ex);
        }
    }
}
