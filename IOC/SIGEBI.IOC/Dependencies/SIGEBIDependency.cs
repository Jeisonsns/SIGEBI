using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Repositories.Auditoria;
using SIGEBI.Persistence.Repositories.Devoluciones;
using SIGEBI.Persistence.Repositories.Notificaciones;
using SIGEBI.Persistence.Repositories.Penalizaciones;
using SIGEBI.Persistence.Repositories.Prestamos;
using SIGEBI.Persistence.Repositories.Recursos;
using SIGEBI.Persistence.Repositories.Usuarios;

namespace SIGEBI.IOC.Dependencies;

public static class SIGEBIDependency
{
    public static IServiceCollection AddSIGEBIDependencies(this IServiceCollection services)
    {
        // Repositorios
        services.AddScoped<IRecursoRepository, RecursoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IPrestamoRepository, PrestamoRepository>();
        services.AddScoped<IDevolucionRepository, DevolucionRepository>();
        services.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
        services.AddScoped<INotificacionRepository, NotificacionRepository>();
        services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

        // Servicios
        services.AddScoped<IRecursoService, RecursoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IPrestamoService, PrestamoService>();
        services.AddScoped<IDevolucionService, DevolucionService>();
        services.AddScoped<IPenalizacionService, PenalizacionService>();
        services.AddScoped<INotificacionService, NotificacionService>();
        services.AddScoped<IAuditoriaService, AuditoriaService>();

        return services;
    }
}
