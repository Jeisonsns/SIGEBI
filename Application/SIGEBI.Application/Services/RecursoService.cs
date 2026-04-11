using SIGEBI.Application.Dtos.Recursos;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Recursos;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class RecursoService : IRecursoService
{
    private readonly IRecursoRepository _recursoRepo;

    public RecursoService(IRecursoRepository recursoRepo)
    {
        _recursoRepo = recursoRepo;
    }

    public async Task<IEnumerable<RecursoDto>> GetAllAsync()
    {
        var recursos = await _recursoRepo.GetAllAsync();
        return recursos.Select(ToDto);
    }

    public async Task<RecursoDto?> GetByIdAsync(string id)
    {
        var r = await _recursoRepo.GetByIdAsync(id);
        return r == null ? null : ToDto(r);
    }

    public async Task<IEnumerable<RecursoDto>> GetByEstadoAsync(EstadoRecurso estado)
    {
        var recursos = await _recursoRepo.GetByEstadoAsync(estado);
        return recursos.Select(ToDto);
    }

    public async Task<OperationResult> SaveAsync(SaveRecursoDto dto)
    {
        try
        {
            var recurso = Recurso.Crear(dto.Titulo, dto.Autor, dto.Isbn,
                dto.Categoria, dto.Editorial, dto.Anio, dto.NumEjemplares);
            await _recursoRepo.AddAsync(recurso);
            return OperationResult.Ok("Recurso registrado correctamente.");
        }
        catch (ArgumentException ex) { return OperationResult.Fail(ex.Message); }
        catch (Exception) { return OperationResult.Fail("Error inesperado al registrar el recurso."); }
    }

    public async Task<OperationResult> UpdateAsync(UpdateRecursoDto dto)
{
    try
    {
        var recurso = await _recursoRepo.GetByIdAsync(dto.Id);
        if (recurso == null) return OperationResult.Fail("Recurso no encontrado.");
        recurso.Actualizar(dto.Titulo, dto.Autor, dto.Isbn,
            dto.Categoria, dto.Editorial, dto.Anio, dto.NumEjemplares);
        await _recursoRepo.UpdateAsync(recurso);
        return OperationResult.Ok("Recurso actualizado correctamente.");
    }
    catch (ArgumentException ex) { return OperationResult.Fail(ex.Message); }
    catch (Exception) { return OperationResult.Fail("Error inesperado al actualizar el recurso."); }
}

public async Task<OperationResult> CambiarEstadoAsync(string id, EstadoRecurso estado)
{
    try
    {
        var recurso = await _recursoRepo.GetByIdAsync(id);
        if (recurso == null) return OperationResult.Fail("Recurso no encontrado.");
        recurso.CambiarEstado(estado);
        await _recursoRepo.UpdateAsync(recurso);
        return OperationResult.Ok("Estado actualizado.");
    }
    catch (InvalidOperationException ex) { return OperationResult.Fail(ex.Message); }
    catch (Exception) { return OperationResult.Fail("Error inesperado al cambiar el estado."); }
}

public async Task<OperationResult> DeleteAsync(string id)
{
    try
    {
        var recurso = await _recursoRepo.GetByIdAsync(id);
        if (recurso == null) return OperationResult.Fail("Recurso no encontrado.");
        if (!recurso.EstaDisponible())
            return OperationResult.Fail("No se puede eliminar un recurso que no está disponible.");
        await _recursoRepo.DeleteAsync(id);
        return OperationResult.Ok("Recurso eliminado.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al eliminar el recurso."); }
}

private static RecursoDto ToDto(Recurso r) => new()
{
    Id = r.Id,
    Titulo = r.Titulo,
    Autor = r.Autor,
    Isbn = r.Isbn,
    Categoria = r.Categoria,
    Editorial = r.Editorial,
    Anio = r.Anio,
    NumEjemplares = r.NumEjemplares,
    Estado = r.Estado
};
}
