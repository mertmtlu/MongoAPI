using MongoAPI.Models.KeyModels;
using MongoDB.Bson;

namespace MongoAPI.Repositories.Interfaces
{
    /// <summary>
    /// Repository for Client entities and operations
    /// </summary>
    public interface IClientRepository : IBaseRepository<Client, ObjectId>
    {
        // Client-specific operations
        Task<Client> GetByNameAsync(string clientName);
        Task<IEnumerable<Client>> GetByTypeAsync(ClientType type);

        // Hierarchy navigation
        Task<IEnumerable<Region>> GetRegionsForClientAsync(ObjectId clientId);
        Task<IEnumerable<TM>> GetAllTMsForClientAsync(ObjectId clientId);

        // Client statistics
        Task<int> GetTotalTMCountForClientAsync(ObjectId clientId);
        Task<Dictionary<string, int>> GetTMCountByRegionAsync(ObjectId clientId);

        // Add regions to a client
        Task AddRegionToClientAsync(ObjectId clientId, Region region);
        Task AddRegionsToClientAsync(ObjectId clientId, IEnumerable<Region> regions);
    }
}
