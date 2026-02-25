using SIGEBI.Domain.Entities.Prestamos;

namespace SIGEBI.Domain.Repository;

public interface IPrestamoRepository : IBaseRepository<Prestamo>
{
    Task<IEnumerable<Prestamo>> GetActivosByUsuarioAsync(string usuarioId);
    Task<IEnumerable<Prestamo>> GetVencidosAsync();
}
