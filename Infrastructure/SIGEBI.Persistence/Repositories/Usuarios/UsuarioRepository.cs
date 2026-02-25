using MongoDB.Driver;
using SIGEBI.Domain.Entities.Usuarios;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Repositories.Usuarios;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(IMongoDatabase database) : base(database, "usuarios") { }

    public async Task<Usuario?> GetByCodigoAsync(string codigo)
        => await _collection.Find(u => u.Codigo == codigo).FirstOrDefaultAsync();
}
