using MongoAPI.Models.KeyModels;
using MongoDB.Bson;

namespace MongoAPI.Repositories.Interfaces
{
    /// <summary>
    /// Repository for Region entities and operations
    /// </summary>
    public interface IRegionRepository : IBaseRepository<Region, ObjectId>
    {
        // Region-specific queries
        Task<Region> GetByRegionNumberAsync(int regionNumber);
        Task<IEnumerable<Region>> GetByHeadquartersAsync(string headquarters);
        Task<IEnumerable<Region>> GetByCityAsync(string city);

        // Hierarchy navigation
        Task<Client> GetClientForRegionAsync(ObjectId regionId);
        Task<IEnumerable<TM>> GetTMsForRegionAsync(ObjectId regionId);

        // Region operations
        Task AddTMToRegionAsync(ObjectId regionId, TM tm);
        Task RemoveTMFromRegionAsync(ObjectId regionId, ObjectId tmId);

        // Statistics
        Task<Dictionary<string, int>> GetTMCountByCityAsync(ObjectId regionId);
    }
}
