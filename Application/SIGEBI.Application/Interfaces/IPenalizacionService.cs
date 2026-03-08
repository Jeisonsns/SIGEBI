using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Penalizaciones;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces;

public interface IPenalizacionService : IBaseService<PenalizacionDto, SavePenalizacionDto, UpdatePenalizacionDto>
{
    Task<IEnumerable<PenalizacionDto>> GetActivasByUsuarioAsync(string usuarioId);
    Task<OperationResult> ResolverAsync(string penalizacionId);
    Task FinalizarVencidasAsync();
}
