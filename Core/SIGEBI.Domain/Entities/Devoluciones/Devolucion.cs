using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Devoluciones;

public class Devolucion : AuditEntity
{
    public string PrestamoId { get; private set; } = string.Empty;
    public DateTime FechaDevolucion { get; private set; }
    public bool EsTardia { get; private set; }
    public int DiasRetraso { get; private set; }

    private Devolucion() { }

    public static Devolucion Registrar(string prestamoId, bool esTardia, int diasRetraso)
    {
        if (string.IsNullOrWhiteSpace(prestamoId)) throw new ArgumentException("El prestamoId es requerido.");

        return new Devolucion
        {
            Id = Guid.NewGuid().ToString(),
            PrestamoId = prestamoId,
            FechaDevolucion = DateTime.UtcNow,
            EsTardia = esTardia,
            DiasRetraso = diasRetraso,
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow
        };
    }
}