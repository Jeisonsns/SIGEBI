using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Auditoria;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces;

public interface IAuditoriaService : IBaseService<AuditoriaDto, SaveAuditoriaDto, UpdateAuditoriaDto>
{
    Task<IEnumerable<AuditoriaDto>> GetByFechaAsync(DateTime desde, DateTime hasta);
    Task<IEnumerable<AuditoriaDto>> GetByUsuarioAsync(string usuario);
    Task RegistrarAsync(string operacion, string usuario, string entidad, string recursoId, string detalles, string resultado);
}
