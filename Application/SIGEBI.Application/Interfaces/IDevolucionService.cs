using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Devoluciones;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces;

public interface IDevolucionService : IBaseService<DevolucionDto, SaveDevolucionDto, UpdateDevolucionDto>
{
    Task<OperationResult> ProcesarDevolucionAsync(string prestamoId);
}
