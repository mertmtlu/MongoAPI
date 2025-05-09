using MongoAPI.Models.Common;
using MongoAPI.Models.KeyModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAPI.Repositories.Interfaces
{
    /// <summary>
    /// Repository for Transformer Station (TM) entities and operations
    /// </summary>
    public interface ITMRepository : IBaseRepository<TM, ObjectId>
    {
        // TM-specific queries
        Task<TM> GetByTMNumberAsync(int tmNumber);
        Task<IEnumerable<TM>> GetByNameAsync(string name);
        Task<IEnumerable<TM>> GetByTypeAsync(TMType type);
        Task<IEnumerable<TM>> GetByVoltageAsync(int voltage);
        Task<IEnumerable<TM>> GetByStateAsync(TMState state);

        // Location-based queries
        Task<IEnumerable<TM>> GetByLocationRadiusAsync(double latitude, double longitude, double radiusKm);
        Task<IEnumerable<TM>> GetByCityAsync(string city);
        Task<IEnumerable<TM>> GetByCountyAsync(string county);

        // Hierarchy navigation
        Task<Region> GetRegionForTMAsync(ObjectId tmId);
        Task<Client> GetClientForTMAsync(ObjectId tmId);
        Task<IEnumerable<Building>> GetBuildingsForTMAsync(ObjectId tmId);

        // Hazard-related queries
        Task<IEnumerable<TM>> GetByFireHazardLevelAsync(Level level);
        Task<IEnumerable<TM>> GetBySecurityHazardScoreAsync(double minScore, double maxScore);
        Task<IEnumerable<TM>> GetByLiquefactionRiskAsync(bool hasLiquefactionRisk);
        // TODO: Add the rest of hazards queries

        // TM operations
        Task AddBuildingToTMAsync(ObjectId tmId, Building building);
        Task UpdateHazardScoreAsync<THazard>(ObjectId tmId, THazard hazard) where THazard : class;

        // Alternative TMs
        Task AddAlternativeTMAsync(ObjectId tmId, ObjectId alternativeTMId);
        Task<IEnumerable<TM>> GetAlternativeTMsAsync(ObjectId tmId);
    }
}
