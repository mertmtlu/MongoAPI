using MongoAPI.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoAPI.Models;

namespace MongoAPI.Repositories.Core
{
    /// <summary>
    /// Base MongoDB repository implementation for common operations
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The ID type (typically ObjectId)</typeparam>
    public class MongoRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : AEntityBase where TId : notnull
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(IMongoDatabase database)
        {
            // Use the entity class name as the collection name (convention-based)
            string collectionName = typeof(TEntity).Name;
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            var objectId = GetObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq("_ID", objectId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }

        public async Task<TId> CreateAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return (TId)(object)entity._ID;
        }

        public async Task UpdateAsync(TId id, TEntity entity)
        {
            var objectId = GetObjectId(id);
            entity._ID = objectId;  // Ensure ID is set correctly
            await _collection.ReplaceOneAsync(doc => doc._ID == objectId, entity);
        }

        public async Task DeleteAsync(TId id)
        {
            var objectId = GetObjectId(id);
            await _collection.DeleteOneAsync(doc => doc._ID == objectId);
        }

        public async Task<bool> ExistsAsync(TId id)
        {
            var objectId = GetObjectId(id);
            var count = await _collection.CountDocumentsAsync(doc => doc._ID == objectId);
            return count > 0;
        }

        public async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TId> ids)
        {
            var objectIds = ids.Select(GetObjectId);
            var filter = Builders<TEntity>.Filter.In("_ID", objectIds);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<TId>> CreateManyAsync(IEnumerable<TEntity> entities)
        {
            await _collection.InsertManyAsync(entities);
            return entities.Select(e => (TId)(object)e._ID);
        }

        public async Task<(IEnumerable<TEntity> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var filter = Builders<TEntity>.Filter.Empty;
            var totalCount = await _collection.CountDocumentsAsync(filter);

            var items = await _collection.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // Helper method to convert the generic TId to ObjectId
        protected ObjectId GetObjectId(TId id)
        {
            if (id is ObjectId objectId)
            {
                return objectId;
            }
            else if (id is string stringId && ObjectId.TryParse(stringId, out var parsedId))
            {
                return parsedId;
            }

            throw new ArgumentException($"Cannot convert {id} to ObjectId");
        }

        // Helper method for filtering documents by a field
        protected async Task<IEnumerable<TEntity>> GetByFieldAsync<TField>(string fieldName, TField value)
        {
            var filter = Builders<TEntity>.Filter.Eq(fieldName, value);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}