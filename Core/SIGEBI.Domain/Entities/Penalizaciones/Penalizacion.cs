using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Penalizaciones;

public class Penalizacion : AuditEntity
{
    public string UsuarioId { get; private set; } = string.Empty;
    public string Causa { get; private set; } = string.Empty;
    public TipoPenalizacion Tipo { get; private set; }
    public EstadoPenalizacion Estado { get; private set; }
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaFin { get; private set; }

    private Penalizacion() { }

    public static Penalizacion Crear(string usuarioId, string causa, TipoPenalizacion tipo, DateTime fechaFin)
    {
        if (string.IsNullOrWhiteSpace(usuarioId)) throw new ArgumentException("El usuarioId es requerido.");
        if (string.IsNullOrWhiteSpace(causa)) throw new ArgumentException("La causa es requerida.");
        if (fechaFin <= DateTime.UtcNow) throw new ArgumentException("La fecha de fin debe ser futura.");

        return new Penalizacion
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = usuarioId,
            Causa = causa,
            Tipo = tipo,
            Estado = EstadoPenalizacion.Activa,
            FechaInicio = DateTime.UtcNow,
            FechaFin = fechaFin,
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow
        };
    }

    public void Resolver()
    {
        if (Estado == EstadoPenalizacion.Resuelta)
            throw new InvalidOperationException("La penalización ya fue resuelta.");
        Estado = EstadoPenalizacion.Resuelta;
        FechaModificacion = DateTime.UtcNow;
    }

    public bool EstaActiva() => Estado == EstadoPenalizacion.Activa && FechaFin > DateTime.UtcNow;
}