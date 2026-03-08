using SIGEBI.Application.Dtos.Recursos;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Recursos;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class RecursoService : IRecursoService
{
    private readonly IRecursoRepository _recursoRepo;
    private readonly IAuditoriaRepository _auditoriaRepo;

    public RecursoService(IRecursoRepository recursoRepo, IAuditoriaRepository auditoriaRepo)
    {
        _recursoRepo = recursoRepo;
        _auditoriaRepo = auditoriaRepo;
    }

    public async Task<IEnumerable<RecursoDto>> GetAllAsync()
    {
        var recursos = await _recursoRepo.GetAllAsync();
        return recursos.Select(r => new RecursoDto
        {
            Id = r.Id, Titulo = r.Titulo, Autor = r.Autor, Isbn = r.Isbn,
            Categoria = r.Categoria, Editorial = r.Editorial,
            Anio = r.Anio, NumEjemplares = r.NumEjemplares, Estado = r.Estado
        });
    }

    public async Task<RecursoDto?> GetByIdAsync(string id)
    {
        var r = await _recursoRepo.GetByIdAsync(id);
        if (r == null) return null;
        return new RecursoDto
        {
            Id = r.Id, Titulo = r.Titulo, Autor = r.Autor, Isbn = r.Isbn,
            Categoria = r.Categoria, Editorial = r.Editorial,
            Anio = r.Anio, NumEjemplares = r.NumEjemplares, Estado = r.Estado
        };
    }

    public async Task<IEnumerable<RecursoDto>> GetByEstadoAsync(EstadoRecurso estado)
    {
        var recursos = await _recursoRepo.GetByEstadoAsync(estado);
        return recursos.Select(r => new RecursoDto
        {
            Id = r.Id, Titulo = r.Titulo, Autor = r.Autor, Isbn = r.Isbn,
            Categoria = r.Categoria, Editorial = r.Editorial,
            Anio = r.Anio, NumEjemplares = r.NumEjemplares, Estado = r.Estado
        });
    }

    public async Task<OperationResult> SaveAsync(SaveRecursoDto dto)
    {
        var recurso = new Recurso
        {
            Id = Guid.NewGuid().ToString(),
            Titulo = dto.Titulo, Autor = dto.Autor, Isbn = dto.Isbn,
            Categoria = dto.Categoria, Editorial = dto.Editorial,
            Anio = dto.Anio, NumEjemplares = dto.NumEjemplares,
            Estado = EstadoRecurso.Disponible
        };
        await _recursoRepo.AddAsync(recurso);
        return OperationResult.Ok("Recurso registrado correctamente.");
    }

    public async Task<OperationResult> UpdateAsync(UpdateRecursoDto dto)
    {
        var recurso = await _recursoRepo.GetByIdAsync(dto.Id);
        if (recurso == null) return OperationResult.Fail("Recurso no encontrado.");

        recurso.Titulo = dto.Titulo; recurso.Autor = dto.Autor;
        recurso.Isbn = dto.Isbn; recurso.Categoria = dto.Categoria;
        recurso.Editorial = dto.Editorial; recurso.Anio = dto.Anio;
        recurso.NumEjemplares = dto.NumEjemplares; recurso.Estado = dto.Estado;

        await _recursoRepo.UpdateAsync(recurso);
        return OperationResult.Ok("Recurso actualizado correctamente.");
    }

    public async Task<OperationResult> CambiarEstadoAsync(string id, EstadoRecurso estado)
    {
        var recurso = await _recursoRepo.GetByIdAsync(id);
        if (recurso == null) return OperationResult.Fail("Recurso no encontrado.");
        recurso.Estado = estado;
        await _recursoRepo.UpdateAsync(recurso);
        return OperationResult.Ok("Estado actualizado.");
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var recurso = await _recursoRepo.GetByIdAsync(id);
        if (recurso == null) return OperationResult.Fail("Recurso no encontrado.");
        if (recurso.Estado == EstadoRecurso.Prestado)
            return OperationResult.Fail("No se puede eliminar un recurso con préstamo activo.");
        await _recursoRepo.DeleteAsync(id);
        return OperationResult.Ok("Recurso eliminado.");
    }
}
