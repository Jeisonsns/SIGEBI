using SIGEBI.Domain.Entities.Devoluciones;

namespace SIGEBI.Domain.Repository;

public interface IDevolucionRepository : IBaseRepository<Devolucion>
{
    Task<Devolucion?> GetByPrestamoAsync(string prestamoId);
}
