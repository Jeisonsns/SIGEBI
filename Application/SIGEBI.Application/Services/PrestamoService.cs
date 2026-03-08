using SIGEBI.Application.Dtos.Prestamos;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Prestamos;
using SIGEBI.Domain.Entities.Recursos;
using SIGEBI.Domain.Entities.Usuarios;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class PrestamoService : IPrestamoService
{
    private readonly IPrestamoRepository _prestamoRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IRecursoRepository _recursoRepo;
    private readonly IPenalizacionRepository _penalizacionRepo;

    private const int LimitePrestamosEstudiante = 3;
    private const int LimitePrestamosDocente = 6;
    private const int DiasPrestamoEstudiante = 7;
    private const int DiasPrestamoDocente = 15;

    public PrestamoService(IPrestamoRepository prestamoRepo, IUsuarioRepository usuarioRepo,
        IRecursoRepository recursoRepo, IPenalizacionRepository penalizacionRepo)
    {
        _prestamoRepo = prestamoRepo;
        _usuarioRepo = usuarioRepo;
        _recursoRepo = recursoRepo;
        _penalizacionRepo = penalizacionRepo;
    }

    public async Task<IEnumerable<PrestamoDto>> GetAllAsync()
    {
        var prestamos = await _prestamoRepo.GetAllAsync();
        return prestamos.Select(p => ToDto(p));
    }

    public async Task<PrestamoDto?> GetByIdAsync(string id)
    {
        var p = await _prestamoRepo.GetByIdAsync(id);
        return p == null ? null : ToDto(p);
    }

    public async Task<IEnumerable<PrestamoDto>> GetActivosByUsuarioAsync(string usuarioId)
    {
        var prestamos = await _prestamoRepo.GetActivosByUsuarioAsync(usuarioId);
        return prestamos.Select(p => ToDto(p));
    }

    public async Task<IEnumerable<PrestamoDto>> GetVencidosAsync()
    {
        var prestamos = await _prestamoRepo.GetVencidosAsync();
        return prestamos.Select(p => ToDto(p));
    }

    public async Task<OperationResult> SaveAsync(SavePrestamoDto dto)
    {
        // Verificar usuario
        var usuario = await _usuarioRepo.GetByIdAsync(dto.UsuarioId);
        if (usuario == null || usuario.Estado == EstadoUsuario.Inactivo)
            return OperationResult.Fail("Usuario no autorizado o inactivo.");

        // Verificar penalizaciones activas
        var penalizaciones = await _penalizacionRepo.GetActivasByUsuarioAsync(dto.UsuarioId);
        if (penalizaciones.Any())
            return OperationResult.Fail("El usuario tiene penalizaciones activas.");

        // Verificar préstamos vencidos
        var vencidos = await _prestamoRepo.GetVencidosAsync();
        if (vencidos.Any(p => p.UsuarioId == dto.UsuarioId))
            return OperationResult.Fail("El usuario tiene préstamos vencidos.");

        // Verificar disponibilidad del recurso
        var recurso = await _recursoRepo.GetByIdAsync(dto.RecursoId);
        if (recurso == null || recurso.Estado != EstadoRecurso.Disponible)
            return OperationResult.Fail("El recurso no está disponible.");

        // Verificar límite de préstamos simultáneos
        var activos = await _prestamoRepo.GetActivosByUsuarioAsync(dto.UsuarioId);
        int limite = usuario.Tipo == TipoUsuario.Docente ? LimitePrestamosDocente : LimitePrestamosEstudiante;
        if (activos.Count() >= limite)
            return OperationResult.Fail("El usuario alcanzó el límite de préstamos simultáneos.");

        // Calcular fecha límite según tipo de usuario
        int dias = usuario.Tipo == TipoUsuario.Docente ? DiasPrestamoDocente : DiasPrestamoEstudiante;
        var prestamo = new Prestamo
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = dto.UsuarioId,
            RecursoId = dto.RecursoId,
            FechaInicio = DateTime.UtcNow,
            FechaLimite = DateTime.UtcNow.AddDays(dias),
            Estado = EstadoPrestamo.Activo
        };

        await _prestamoRepo.AddAsync(prestamo);

        recurso.Estado = EstadoRecurso.Prestado;
        await _recursoRepo.UpdateAsync(recurso);

        return OperationResult.Ok("Préstamo autorizado correctamente.");
    }

    public async Task<OperationResult> UpdateAsync(UpdatePrestamoDto dto)
    {
        var prestamo = await _prestamoRepo.GetByIdAsync(dto.Id);
        if (prestamo == null) return OperationResult.Fail("Préstamo no encontrado.");
        prestamo.Estado = dto.Estado;
        await _prestamoRepo.UpdateAsync(prestamo);
        return OperationResult.Ok("Préstamo actualizado.");
    }

    public async Task<OperationResult> RenovarAsync(string prestamoId)
    {
        var prestamo = await _prestamoRepo.GetByIdAsync(prestamoId);
        if (prestamo == null) return OperationResult.Fail("Préstamo no encontrado.");
        if (prestamo.Estado != EstadoPrestamo.Activo)
            return OperationResult.Fail("Solo se pueden renovar préstamos activos.");
        if (prestamo.FechaLimite < DateTime.UtcNow)
            return OperationResult.Fail("No se puede renovar un préstamo vencido.");

        var usuario = await _usuarioRepo.GetByIdAsync(prestamo.UsuarioId);
        int dias = usuario?.Tipo == TipoUsuario.Docente ? DiasPrestamoDocente : DiasPrestamoEstudiante;
        prestamo.FechaLimite = prestamo.FechaLimite.AddDays(dias);
        await _prestamoRepo.UpdateAsync(prestamo);
        return OperationResult.Ok("Préstamo renovado correctamente.");
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var prestamo = await _prestamoRepo.GetByIdAsync(id);
        if (prestamo == null) return OperationResult.Fail("Préstamo no encontrado.");
        await _prestamoRepo.DeleteAsync(id);
        return OperationResult.Ok("Préstamo eliminado.");
    }

    private static PrestamoDto ToDto(Prestamo p) => new()
    {
        Id = p.Id, UsuarioId = p.UsuarioId, RecursoId = p.RecursoId,
        FechaInicio = p.FechaInicio, FechaLimite = p.FechaLimite, Estado = p.Estado
    };
}
