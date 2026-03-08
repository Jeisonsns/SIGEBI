using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Prestamos;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces;

public interface IPrestamoService : IBaseService<PrestamoDto, SavePrestamoDto, UpdatePrestamoDto>
{
    Task<IEnumerable<PrestamoDto>> GetActivosByUsuarioAsync(string usuarioId);
    Task<IEnumerable<PrestamoDto>> GetVencidosAsync();
    Task<OperationResult> RenovarAsync(string prestamoId);
}
