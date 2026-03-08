using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Base;

public interface IBaseService<TDto, TSaveDto, TUpdateDto>
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto?> GetByIdAsync(string id);
    Task<OperationResult> SaveAsync(TSaveDto dto);
    Task<OperationResult> UpdateAsync(TUpdateDto dto);
    Task<OperationResult> DeleteAsync(string id);
}
