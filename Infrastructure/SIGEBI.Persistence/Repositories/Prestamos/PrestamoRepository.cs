using MongoDB.Driver;
using SIGEBI.Domain.Entities.Prestamos;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Prestamos;

public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
{
    public PrestamoRepository(IMongoDatabase database) : base(database, "prestamos") { }

    public async Task<IEnumerable<Prestamo>> GetActivosByUsuarioAsync(string usuarioId)
        => await _collection.Find(p => p.UsuarioId == usuarioId && p.Estado == EstadoPrestamo.Activo).ToListAsync();

    public async Task<IEnumerable<Prestamo>> GetVencidosAsync()
        => await _collection.Find(p => p.Estado == EstadoPrestamo.Activo && p.FechaLimite < DateTime.UtcNow).ToListAsync();
}
