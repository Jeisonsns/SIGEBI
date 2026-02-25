using MongoDB.Driver;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : AuditEntity
{
    protected readonly IMongoCollection<T> _collection;

    public BaseRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }

    public async Task<T?> GetByIdAsync(string id)
        => await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _collection.Find(_ => true).ToListAsync();

    public async Task AddAsync(T entity)
    {
        entity.FechaCreacion = DateTime.UtcNow;
        entity.FechaModificacion = DateTime.UtcNow;
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
    }

    public async Task DeleteAsync(string id)
        => await _collection.DeleteOneAsync(e => e.Id == id);
}
