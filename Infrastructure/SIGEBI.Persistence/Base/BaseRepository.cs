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
    {
        try
        {
            return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Error al obtener el registro con id {id}.", ex);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al obtener los registros.", ex);
        }
    }

    public async Task AddAsync(T entity)
    {
        try
        {
            await _collection.InsertOneAsync(entity);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al insertar el registro.", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Error al actualizar el registro.", ex);
        }
    }

    public async Task DeleteAsync(string id)
    {
        try
        {
            await _collection.DeleteOneAsync(e => e.Id == id);
        }
        catch (Exception ex)
        {
            throw new RepositoryException($"Error al eliminar el registro con id {id}.", ex);
        }
    }
}
