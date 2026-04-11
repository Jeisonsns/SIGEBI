using SIGEBI.Application.Dtos.Penalizaciones;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Penalizaciones;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class PenalizacionService : IPenalizacionService
{
    private readonly IPenalizacionRepository _penalizacionRepo;

    public PenalizacionService(IPenalizacionRepository penalizacionRepo)
    {
        _penalizacionRepo = penalizacionRepo;
    }

    public async Task<IEnumerable<PenalizacionDto>> GetAllAsync()
        => (await _penalizacionRepo.GetAllAsync()).Select(ToDto);

    public async Task<PenalizacionDto?> GetByIdAsync(string id)
    {
        var p = await _penalizacionRepo.GetByIdAsync(id);
        return p == null ? null : ToDto(p);
    }

    public async Task<IEnumerable<PenalizacionDto>> GetActivasByUsuarioAsync(string usuarioId)
        => (await _penalizacionRepo.GetActivasByUsuarioAsync(usuarioId)).Select(ToDto);

    public async Task<OperationResult> SaveAsync(SavePenalizacionDto dto)
    {
        try
        {
            var penalizacion = Penalizacion.Crear(dto.UsuarioId, dto.Causa, dto.Tipo, dto.FechaFin);
            await _penalizacionRepo.AddAsync(penalizacion);
            return OperationResult.Ok("Penalización aplicada.");
        }
        catch (ArgumentException ex) { return OperationResult.Fail(ex.Message); }
        catch (Exception) { return OperationResult.Fail("Error inesperado al aplicar la penalización."); }
    }

    public async Task<OperationResult> UpdateAsync(UpdatePenalizacionDto dto)
{
    try
    {
        var penalizacion = await _penalizacionRepo.GetByIdAsync(dto.Id);
        if (penalizacion == null) return OperationResult.Fail("Penalización no encontrada.");
        await _penalizacionRepo.UpdateAsync(penalizacion);
        return OperationResult.Ok("Penalización actualizada.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al actualizar la penalización."); }
}

public async Task<OperationResult> ResolverAsync(string penalizacionId)
{
    try
    {
        var penalizacion = await _penalizacionRepo.GetByIdAsync(penalizacionId);
        if (penalizacion == null) return OperationResult.Fail("Penalización no encontrada.");
        penalizacion.Resolver();
        await _penalizacionRepo.UpdateAsync(penalizacion);
        return OperationResult.Ok("Penalización resuelta.");
    }
    catch (InvalidOperationException ex) { return OperationResult.Fail(ex.Message); }
    catch (Exception) { return OperationResult.Fail("Error inesperado al resolver la penalización."); }
}

public async Task FinalizarVencidasAsync()
{
    var todas = await _penalizacionRepo.GetAllAsync();
    foreach (var p in todas.Where(p => p.EstaActiva() && p.FechaFin <= DateTime.UtcNow))
    {
        p.Resolver();
        await _penalizacionRepo.UpdateAsync(p);
    }
}

public async Task<OperationResult> DeleteAsync(string id)
{
    try
    {
        var penalizacion = await _penalizacionRepo.GetByIdAsync(id);
        if (penalizacion == null) return OperationResult.Fail("Penalización no encontrada.");
        await _penalizacionRepo.DeleteAsync(id);
        return OperationResult.Ok("Penalización eliminada.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al eliminar la penalización."); }
}

private static PenalizacionDto ToDto(Penalizacion p) => new()
{
    Id = p.Id,
    UsuarioId = p.UsuarioId,
    Causa = p.Causa,
    Tipo = p.Tipo,
    Estado = p.Estado,
    FechaInicio = p.FechaInicio,
    FechaFin = p.FechaFin
};
}
