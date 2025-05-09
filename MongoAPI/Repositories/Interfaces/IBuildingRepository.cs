using MongoAPI.Models.Block;
using MongoAPI.Models.KeyModels;
using MongoDB.Bson;

namespace MongoAPI.Repositories.Interfaces
{
    /// <summary>
    /// Repository for Building entities and operations
    /// </summary>
    public interface IBuildingRepository : IBaseRepository<Building, ObjectId>
    {
        // Building-specific queries
        Task<Building> GetByBuildingNumberAsync(int buildingNumber);
        Task<IEnumerable<Building>> GetByTypeAsync(BuildingType type);
        Task<IEnumerable<Building>> GetByMETUScopeAsync(bool inScope);

        // Hierarchy navigation
        Task<TM> GetTMForBuildingAsync(ObjectId buildingId);
        Task<Region> GetRegionForBuildingAsync(ObjectId buildingId);
        Task<IEnumerable<ABlock>> GetBlocksForBuildingAsync(ObjectId buildingId);

        // Building operations
        Task AddBlockToBuildingAsync(ObjectId buildingId, ABlock block);
        Task RemoveBlockFromBuildingAsync(ObjectId buildingId, ObjectId blockId);

        // Statistics
        Task<Dictionary<BuildingType, int>> GetBuildingCountByTypeForTMAsync(ObjectId tmId);
    }
}
