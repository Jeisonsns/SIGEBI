using AuditoriaEntity = SIGEBI.Domain.Entities.Auditoria.Auditoria;
using SIGEBI.Application.Dtos.Auditoria;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _auditoriaRepo;

    public AuditoriaService(IAuditoriaRepository auditoriaRepo)
    {
        _auditoriaRepo = auditoriaRepo;
    }

    public async Task<IEnumerable<AuditoriaDto>> GetAllAsync()
        => (await _auditoriaRepo.GetAllAsync()).Select(ToDto);

    public async Task<AuditoriaDto?> GetByIdAsync(string id)
    {
        var a = await _auditoriaRepo.GetByIdAsync(id);
        return a == null ? null : ToDto(a);
    }

    public async Task<IEnumerable<AuditoriaDto>> GetByFechaAsync(DateTime desde, DateTime hasta)
        => (await _auditoriaRepo.GetByFechaAsync(desde, hasta)).Select(ToDto);

    public async Task<IEnumerable<AuditoriaDto>> GetByUsuarioAsync(string usuario)
        => (await _auditoriaRepo.GetByUsuarioAsync(usuario)).Select(ToDto);

    public async Task RegistrarAsync(string operacion, string usuario, string entidad,
        string recursoId, string detalles, string resultado)
    {
        try
        {
            var auditoria = AuditoriaEntity.Registrar(operacion, usuario, entidad, recursoId, detalles, resultado);
            await _auditoriaRepo.AddAsync(auditoria);
        }
        catch { }
    }

    public async Task<OperationResult> SaveAsync(SaveAuditoriaDto dto)
    {
        try
        {
            await RegistrarAsync(dto.Operacion, dto.Usuario, dto.Entidad, dto.RecursoId, dto.Detalles, dto.Resultado);
            return OperationResult.Ok("Registro de auditoría guardado.");
        }
        catch (Exception) { return OperationResult.Fail("Error inesperado al guardar el registro."); }
    }

    public async Task<OperationResult> UpdateAsync(UpdateAuditoriaDto dto)
    {
        try
        {
            var a = await _auditoriaRepo.GetByIdAsync(dto.Id);
            if (a == null) return OperationResult.Fail("Registro no encontrado.");
            await _auditoriaRepo.UpdateAsync(a);
            return OperationResult.Ok("Registro actualizado.");
        }
        catch (Exception) { return OperationResult.Fail("Error inesperado al actualizar el registro."); }
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        try
        {
            var a = await _auditoriaRepo.GetByIdAsync(id);
            if (a == null) return OperationResult.Fail("Registro no encontrado.");
            await _auditoriaRepo.DeleteAsync(id);
            return OperationResult.Ok("Registro eliminado.");
        }
        catch (Exception) { return OperationResult.Fail("Error inesperado al eliminar el registro."); }
    }

    private static AuditoriaDto ToDto(AuditoriaEntity a) => new()
    {
        Id = a.Id,
        Operacion = a.Operacion,
        Usuario = a.Usuario,
        Entidad = a.Entidad,
        RecursoId = a.RecursoId,
        Fecha = a.Fecha,
        Detalles = a.Detalles,
        Resultado = a.Resultado
    };
}