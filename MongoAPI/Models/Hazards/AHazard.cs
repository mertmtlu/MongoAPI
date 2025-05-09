using System.ComponentModel.DataAnnotations;
using MongoAPI.Models.Common;

namespace MongoAPI.Models.Hazards
{
    public abstract class AHazard<T> where T : Enum
    {
        [Range(0, 1)] public double Score { get; set; }
        public Level Level { get; set; }
        public Dictionary<T, int> EliminationCosts { get; set; } = new();
        public required bool PreviousIncidentOccurred { get; set; }
        public string PreviousIncidentDescription { get; set; } = string.Empty;
        public required double DistanceToInventory { get; set; }
    }
}
