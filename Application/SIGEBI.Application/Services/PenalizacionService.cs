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
    {
        var penalizaciones = await _penalizacionRepo.GetAllAsync();
        return penalizaciones.Select(p => ToDto(p));
    }

    public async Task<PenalizacionDto?> GetByIdAsync(string id)
    {
        var p = await _penalizacionRepo.GetByIdAsync(id);
        return p == null ? null : ToDto(p);
    }

    public async Task<IEnumerable<PenalizacionDto>> GetActivasByUsuarioAsync(string usuarioId)
    {
        var penalizaciones = await _penalizacionRepo.GetActivasByUsuarioAsync(usuarioId);
        return penalizaciones.Select(p => ToDto(p));
    }

    public async Task<OperationResult> SaveAsync(SavePenalizacionDto dto)
    {
        var penalizacion = new Penalizacion
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = dto.UsuarioId, Causa = dto.Causa,
            Tipo = dto.Tipo, Estado = EstadoPenalizacion.Activa,
            FechaInicio = DateTime.UtcNow, FechaFin = dto.FechaFin
        };
        await _penalizacionRepo.AddAsync(penalizacion);
        return OperationResult.Ok("Penalización aplicada.");
    }

    public async Task<OperationResult> UpdateAsync(UpdatePenalizacionDto dto)
    {
        var penalizacion = await _penalizacionRepo.GetByIdAsync(dto.Id);
        if (penalizacion == null) return OperationResult.Fail("Penalización no encontrada.");
        penalizacion.Estado = dto.Estado;
        await _penalizacionRepo.UpdateAsync(penalizacion);
        return OperationResult.Ok("Penalización actualizada.");
    }

    public async Task<OperationResult> ResolverAsync(string penalizacionId)
    {
        var penalizacion = await _penalizacionRepo.GetByIdAsync(penalizacionId);
        if (penalizacion == null) return OperationResult.Fail("Penalización no encontrada.");
        penalizacion.Estado = EstadoPenalizacion.Resuelta;
        await _penalizacionRepo.UpdateAsync(penalizacion);
        return OperationResult.Ok("Penalización resuelta.");
    }

    public async Task FinalizarVencidasAsync()
    {
        var todas = await _penalizacionRepo.GetAllAsync();
        foreach (var p in todas.Where(p => p.Estado == EstadoPenalizacion.Activa && p.FechaFin <= DateTime.UtcNow))
        {
            p.Estado = EstadoPenalizacion.Resuelta;
            await _penalizacionRepo.UpdateAsync(p);
        }
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var penalizacion = await _penalizacionRepo.GetByIdAsync(id);
        if (penalizacion == null) return OperationResult.Fail("Penalización no encontrada.");
        await _penalizacionRepo.DeleteAsync(id);
        return OperationResult.Ok("Penalización eliminada.");
    }

    private static PenalizacionDto ToDto(Penalizacion p) => new()
    {
        Id = p.Id, UsuarioId = p.UsuarioId, Causa = p.Causa,
        Tipo = p.Tipo, Estado = p.Estado,
        FechaInicio = p.FechaInicio, FechaFin = p.FechaFin
    };
}
