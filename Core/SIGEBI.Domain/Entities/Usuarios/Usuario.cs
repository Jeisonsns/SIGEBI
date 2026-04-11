using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Usuarios;

public class Usuario : AuditEntity
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public TipoUsuario Tipo { get; private set; }
    public EstadoUsuario Estado { get; private set; }
    public DateTime FechaRegistro { get; private set; }

    private Usuario() { }

    public static Usuario Crear(string codigo, string nombre, string email, TipoUsuario tipo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new ArgumentException("El código es requerido.");
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El email es requerido.");

        return new Usuario
        {
            Id = Guid.NewGuid().ToString(),
            Codigo = codigo,
            Nombre = nombre,
            Email = email,
            Tipo = tipo,
            Estado = EstadoUsuario.Activo,
            FechaRegistro = DateTime.UtcNow,
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow
        };
    }

    public void Actualizar(string nombre, string email)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El email es requerido.");
        Nombre = nombre;
        Email = email;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Activar()
    {
        Estado = EstadoUsuario.Activo;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Estado = EstadoUsuario.Inactivo;
        FechaModificacion = DateTime.UtcNow;
    }

    public bool EstaActivo() => Estado == EstadoUsuario.Activo;

    public int LimitePrestamos() => Tipo == TipoUsuario.Docente ? 6 : 3;

    public int DiasPrestamoPermitidos() => Tipo == TipoUsuario.Docente ? 15 : 7;
}