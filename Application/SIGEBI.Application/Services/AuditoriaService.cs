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
    {
        var registros = await _auditoriaRepo.GetAllAsync();
        return registros.Select(a => ToDto(a));
    }

    public async Task<AuditoriaDto?> GetByIdAsync(string id)
    {
        var a = await _auditoriaRepo.GetByIdAsync(id);
        return a == null ? null : ToDto(a);
    }

    public async Task<IEnumerable<AuditoriaDto>> GetByFechaAsync(DateTime desde, DateTime hasta)
    {
        var registros = await _auditoriaRepo.GetByFechaAsync(desde, hasta);
        return registros.Select(a => ToDto(a));
    }

    public async Task<IEnumerable<AuditoriaDto>> GetByUsuarioAsync(string usuario)
    {
        var registros = await _auditoriaRepo.GetByUsuarioAsync(usuario);
        return registros.Select(a => ToDto(a));
    }

    public async Task RegistrarAsync(string operacion, string usuario, string entidad, string recursoId, string detalles, string resultado)
    {
        var auditoria = new AuditoriaEntity
        {
            Id = Guid.NewGuid().ToString(),
            Operacion = operacion, Usuario = usuario,
            Entidad = entidad, RecursoId = recursoId,
            Fecha = DateTime.UtcNow, Detalles = detalles,
            Resultado = resultado
        };
        await _auditoriaRepo.AddAsync(auditoria);
    }

    public async Task<OperationResult> SaveAsync(SaveAuditoriaDto dto)
    {
        await RegistrarAsync(dto.Operacion, dto.Usuario, dto.Entidad, dto.RecursoId, dto.Detalles, dto.Resultado);
        return OperationResult.Ok("Registro de auditoría guardado.");
    }

    public async Task<OperationResult> UpdateAsync(UpdateAuditoriaDto dto)
    {
        var a = await _auditoriaRepo.GetByIdAsync(dto.Id);
        if (a == null) return OperationResult.Fail("Registro no encontrado.");
        a.Resultado = dto.Resultado;
        await _auditoriaRepo.UpdateAsync(a);
        return OperationResult.Ok("Registro actualizado.");
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var a = await _auditoriaRepo.GetByIdAsync(id);
        if (a == null) return OperationResult.Fail("Registro no encontrado.");
        await _auditoriaRepo.DeleteAsync(id);
        return OperationResult.Ok("Registro eliminado.");
    }

    private static AuditoriaDto ToDto(AuditoriaEntity a) => new()
    {
        Id = a.Id, Operacion = a.Operacion, Usuario = a.Usuario,
        Entidad = a.Entidad, RecursoId = a.RecursoId,
        Fecha = a.Fecha, Detalles = a.Detalles, Resultado = a.Resultado
    };
}
