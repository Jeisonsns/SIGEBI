using SIGEBI.Application.Dtos.Devoluciones;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Devoluciones;
using SIGEBI.Domain.Entities.Penalizaciones;
using SIGEBI.Domain.Entities.Prestamos;
using SIGEBI.Domain.Entities.Recursos;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Application.Services;

public class DevolucionService : IDevolucionService
{
    private readonly IDevolucionRepository _devolucionRepo;
    private readonly IPrestamoRepository _prestamoRepo;
    private readonly IRecursoRepository _recursoRepo;
    private readonly IPenalizacionRepository _penalizacionRepo;

    public DevolucionService(IDevolucionRepository devolucionRepo, IPrestamoRepository prestamoRepo,
        IRecursoRepository recursoRepo, IPenalizacionRepository penalizacionRepo)
    {
        _devolucionRepo = devolucionRepo;
        _prestamoRepo = prestamoRepo;
        _recursoRepo = recursoRepo;
        _penalizacionRepo = penalizacionRepo;
    }

    public async Task<IEnumerable<DevolucionDto>> GetAllAsync()
    {
        var devoluciones = await _devolucionRepo.GetAllAsync();
        return devoluciones.Select(d => ToDto(d));
    }

    public async Task<DevolucionDto?> GetByIdAsync(string id)
    {
        var d = await _devolucionRepo.GetByIdAsync(id);
        return d == null ? null : ToDto(d);
    }

    public async Task<OperationResult> ProcesarDevolucionAsync(string prestamoId)
    {
        var prestamo = await _prestamoRepo.GetByIdAsync(prestamoId);
        if (prestamo == null || prestamo.Estado != EstadoPrestamo.Activo)
            return OperationResult.Fail("No existe un préstamo activo con ese id.");

        bool esTardia = DateTime.UtcNow > prestamo.FechaLimite;

        var devolucion = new Devolucion
        {
            Id = Guid.NewGuid().ToString(),
            PrestamoId = prestamoId,
            FechaDevolucion = DateTime.UtcNow,
            EsTardia = esTardia
        };
        await _devolucionRepo.AddAsync(devolucion);

        // Marcar préstamo como finalizado
        prestamo.Estado = EstadoPrestamo.Finalizado;
        await _prestamoRepo.UpdateAsync(prestamo);

        // Actualizar estado del recurso a disponible
        var recurso = await _recursoRepo.GetByIdAsync(prestamo.RecursoId);
        if (recurso != null)
        {
            recurso.Estado = EstadoRecurso.Disponible;
            await _recursoRepo.UpdateAsync(recurso);
        }

        // Aplicar penalización si es tardía
        if (esTardia)
        {
            int diasRetraso = (int)(DateTime.UtcNow - prestamo.FechaLimite).TotalDays;
            var penalizacion = new Penalizacion
            {
                Id = Guid.NewGuid().ToString(),
                UsuarioId = prestamo.UsuarioId,
                Causa = $"Devolución tardía con {diasRetraso} día(s) de retraso.",
                Tipo = TipoPenalizacion.SuspensionTemporal,
                Estado = EstadoPenalizacion.Activa,
                FechaInicio = DateTime.UtcNow,
                FechaFin = DateTime.UtcNow.AddDays(diasRetraso * 2)
            };
            await _penalizacionRepo.AddAsync(penalizacion);
            return OperationResult.Ok("Devolución tardía registrada. Se aplicó una penalización.");
        }

        return OperationResult.Ok("Devolución registrada correctamente.");
    }

    public async Task<OperationResult> SaveAsync(SaveDevolucionDto dto)
        => await ProcesarDevolucionAsync(dto.PrestamoId);

    public async Task<OperationResult> UpdateAsync(UpdateDevolucionDto dto)
    {
        var devolucion = await _devolucionRepo.GetByIdAsync(dto.Id);
        if (devolucion == null) return OperationResult.Fail("Devolución no encontrada.");
        devolucion.EsTardia = dto.EsTardia;
        await _devolucionRepo.UpdateAsync(devolucion);
        return OperationResult.Ok("Devolución actualizada.");
    }

    public async Task<OperationResult> DeleteAsync(string id)
    {
        var devolucion = await _devolucionRepo.GetByIdAsync(id);
        if (devolucion == null) return OperationResult.Fail("Devolución no encontrada.");
        await _devolucionRepo.DeleteAsync(id);
        return OperationResult.Ok("Devolución eliminada.");
    }

    private static DevolucionDto ToDto(Devolucion d) => new()
    {
        Id = d.Id, PrestamoId = d.PrestamoId,
        FechaDevolucion = d.FechaDevolucion, EsTardia = d.EsTardia
    };
}
