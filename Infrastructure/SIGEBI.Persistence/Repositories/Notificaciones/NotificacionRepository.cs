using MongoDB.Driver;
using SIGEBI.Domain.Entities.Notificaciones;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Notificaciones;

public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
{
    public NotificacionRepository(IMongoDatabase database) : base(database, "notificaciones") { }

    public async Task<IEnumerable<Notificacion>> GetByUsuarioAsync(string usuarioId)
        => await _collection.Find(n => n.UsuarioId == usuarioId).ToListAsync();
}
