using SIGEBI.Application.Dtos.Devoluciones;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Devoluciones;
using SIGEBI.Domain.Entities.Penalizaciones;
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
        => (await _devolucionRepo.GetAllAsync()).Select(ToDto);

    public async Task<DevolucionDto?> GetByIdAsync(string id)
    {
        var d = await _devolucionRepo.GetByIdAsync(id);
        return d == null ? null : ToDto(d);
    }

    public async Task<OperationResult> ProcesarDevolucionAsync(string prestamoId)
    {
        try
        {
            var prestamo = await _prestamoRepo.GetByIdAsync(prestamoId);
            if (prestamo == null || !prestamo.EstaActivo())
                return OperationResult.Fail("No existe un préstamo activo con ese id.");

            bool esTardia = prestamo.EstaVencido();
int diasRetraso = prestamo.DiasDeRetraso();

var devolucion = Devolucion.Registrar(prestamoId, esTardia, diasRetraso);
await _devolucionRepo.AddAsync(devolucion);

prestamo.Finalizar();
await _prestamoRepo.UpdateAsync(prestamo);

var recurso = await _recursoRepo.GetByIdAsync(prestamo.RecursoId);
if (recurso != null)
{
    recurso.MarcarComoDisponible();
    await _recursoRepo.UpdateAsync(recurso);
}

if (esTardia)
{
    var penalizacion = Penalizacion.Crear(
        prestamo.UsuarioId,
        $"Devolución tardía con {diasRetraso} día(s) de retraso.",
        TipoPenalizacion.SuspensionTemporal,
        DateTime.UtcNow.AddDays(diasRetraso * 2));
    await _penalizacionRepo.AddAsync(penalizacion);
    return OperationResult.Ok("Devolución tardía registrada. Se aplicó una penalización.");
}

return OperationResult.Ok("Devolución registrada correctamente.");
        }
        catch (InvalidOperationException ex) { return OperationResult.Fail(ex.Message); }
        catch (Exception) { return OperationResult.Fail("Error inesperado al procesar la devolución."); }
    }

    public async Task<OperationResult> SaveAsync(SaveDevolucionDto dto)
        => await ProcesarDevolucionAsync(dto.PrestamoId);

public async Task<OperationResult> UpdateAsync(UpdateDevolucionDto dto)
{
    try
    {
        var devolucion = await _devolucionRepo.GetByIdAsync(dto.Id);
        if (devolucion == null) return OperationResult.Fail("Devolución no encontrada.");
        await _devolucionRepo.UpdateAsync(devolucion);
        return OperationResult.Ok("Devolución actualizada.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al actualizar la devolución."); }
}

public async Task<OperationResult> DeleteAsync(string id)
{
    try
    {
        var devolucion = await _devolucionRepo.GetByIdAsync(id);
        if (devolucion == null) return OperationResult.Fail("Devolución no encontrada.");
        await _devolucionRepo.DeleteAsync(id);
        return OperationResult.Ok("Devolución eliminada.");
    }
    catch (Exception) { return OperationResult.Fail("Error inesperado al eliminar la devolución."); }
}

private static DevolucionDto ToDto(Devolucion d) => new()
{
    Id = d.Id,
    PrestamoId = d.PrestamoId,
    FechaDevolucion = d.FechaDevolucion,
    EsTardia = d.EsTardia
};
}
