using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Recursos;

public class Recurso : AuditEntity
{
    public string Titulo { get; private set; } = string.Empty;
    public string Autor { get; private set; } = string.Empty;
    public string Isbn { get; private set; } = string.Empty;
    public string Categoria { get; private set; } = string.Empty;
    public string Editorial { get; private set; } = string.Empty;
    public int Anio { get; private set; }
    public int NumEjemplares { get; private set; }
    public EstadoRecurso Estado { get; private set; }

    private Recurso() { }

    public static Recurso Crear(string titulo, string autor, string isbn,
        string categoria, string editorial, int anio, int numEjemplares)
    {
        if (string.IsNullOrWhiteSpace(titulo)) throw new ArgumentException("El título es requerido.");
        if (string.IsNullOrWhiteSpace(autor)) throw new ArgumentException("El autor es requerido.");
        if (numEjemplares <= 0) throw new ArgumentException("El número de ejemplares debe ser mayor a cero.");

        return new Recurso
        {
            Id = Guid.NewGuid().ToString(),
            Titulo = titulo,
            Autor = autor,
            Isbn = isbn,
            Categoria = categoria,
            Editorial = editorial,
            Anio = anio,
            NumEjemplares = numEjemplares,
            Estado = EstadoRecurso.Disponible,
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow
        };
    }

    public void Actualizar(string titulo, string autor, string isbn,
        string categoria, string editorial, int anio, int numEjemplares)
    {
        if (string.IsNullOrWhiteSpace(titulo)) throw new ArgumentException("El título es requerido.");
        if (numEjemplares <= 0) throw new ArgumentException("El número de ejemplares debe ser mayor a cero.");

        Titulo = titulo; Autor = autor; Isbn = isbn;
        Categoria = categoria; Editorial = editorial;
        Anio = anio; NumEjemplares = numEjemplares;
        FechaModificacion = DateTime.UtcNow;
    }

    public void CambiarEstado(EstadoRecurso nuevoEstado)
    {
        if (Estado == EstadoRecurso.Prestado && nuevoEstado == EstadoRecurso.FueraDeServicio)
            throw new InvalidOperationException("No se puede poner fuera de servicio un recurso con préstamo activo.");
        Estado = nuevoEstado;
        FechaModificacion = DateTime.UtcNow;
    }

    public void MarcarComoPrestado()
    {
        if (Estado != EstadoRecurso.Disponible)
            throw new InvalidOperationException("El recurso no está disponible para préstamo.");
        Estado = EstadoRecurso.Prestado;
        FechaModificacion = DateTime.UtcNow;
    }

    public void MarcarComoDisponible()
    {
        Estado = EstadoRecurso.Disponible;
        FechaModificacion = DateTime.UtcNow;
    }

    public bool EstaDisponible() => Estado == EstadoRecurso.Disponible;
}