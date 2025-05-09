namespace MongoAPI.Repositories.Interfaces
{
    /// <summary>
    /// Base repository interface defining common operations for all entity types
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The ID type (typically ObjectId)</typeparam>
    public interface IBaseRepository<TEntity, TId> where TEntity : class
    {
        // Basic CRUD operations
        Task<TEntity> GetByIdAsync(TId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TId> CreateAsync(TEntity entity);
        Task UpdateAsync(TId id, TEntity entity);
        Task DeleteAsync(TId id);
        Task<bool> ExistsAsync(TId id);

        // Batch operations
        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TId> ids);
        Task<IEnumerable<TId>> CreateManyAsync(IEnumerable<TEntity> entities);

        // Pagination support
        Task<(IEnumerable<TEntity> Items, long TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    }
}
