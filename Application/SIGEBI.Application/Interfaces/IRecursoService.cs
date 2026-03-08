using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Recursos;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Recursos;

namespace SIGEBI.Application.Interfaces;

public interface IRecursoService : IBaseService<RecursoDto, SaveRecursoDto, UpdateRecursoDto>
{
    Task<IEnumerable<RecursoDto>> GetByEstadoAsync(EstadoRecurso estado);
    Task<OperationResult> CambiarEstadoAsync(string id, EstadoRecurso estado);
}
