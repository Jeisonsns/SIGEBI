using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Prestamos;

public class Prestamo : AuditEntity
{
    public string UsuarioId { get; private set; } = string.Empty;
    public string RecursoId { get; private set; } = string.Empty;
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaLimite { get; private set; }
    public EstadoPrestamo Estado { get; private set; }

    private Prestamo() { }

    public static Prestamo Crear(string usuarioId, string recursoId, int diasPrestamo)
    {
        if (string.IsNullOrWhiteSpace(usuarioId)) throw new ArgumentException("El usuarioId es requerido.");
        if (string.IsNullOrWhiteSpace(recursoId)) throw new ArgumentException("El recursoId es requerido.");
        if (diasPrestamo <= 0) throw new ArgumentException("Los días de préstamo deben ser mayor a cero.");

        return new Prestamo
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = usuarioId, RecursoId = recursoId,
            FechaInicio = DateTime.UtcNow,
            FechaLimite = DateTime.UtcNow.AddDays(diasPrestamo),
            Estado = EstadoPrestamo.Activo,
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow
        };
    }

    public void Renovar(int diasAdicionales)
    {
        if (!EstaActivo()) throw new InvalidOperationException("Solo se pueden renovar préstamos activos.");
        if (EstaVencido()) throw new InvalidOperationException("No se puede renovar un préstamo vencido.");
        FechaLimite = FechaLimite.AddDays(diasAdicionales);
        FechaModificacion = DateTime.UtcNow;
    }

    public void Finalizar()
    {
        if (!EstaActivo()) throw new InvalidOperationException("El préstamo no está activo.");
        Estado = EstadoPrestamo.Finalizado;
        FechaModificacion = DateTime.UtcNow;
    }

    public void MarcarComoVencido()
    {
        if (!EstaActivo()) throw new InvalidOperationException("El préstamo no está activo.");
        Estado = EstadoPrestamo.Vencido;
        FechaModificacion = DateTime.UtcNow;
    }

    public bool EstaActivo() => Estado == EstadoPrestamo.Activo;
    public bool EstaVencido() => FechaLimite < DateTime.UtcNow;
    public int DiasDeRetraso() => EstaVencido() ? (int)(DateTime.UtcNow - FechaLimite).TotalDays : 0;
}