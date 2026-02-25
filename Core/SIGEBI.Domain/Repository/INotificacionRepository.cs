using SIGEBI.Domain.Entities.Notificaciones;

namespace SIGEBI.Domain.Repository;

public interface INotificacionRepository : IBaseRepository<Notificacion>
{
    Task<IEnumerable<Notificacion>> GetByUsuarioAsync(string usuarioId);
}
