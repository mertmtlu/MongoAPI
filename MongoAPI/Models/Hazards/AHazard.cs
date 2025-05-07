using System.ComponentModel.DataAnnotations;
using MongoAPI.Models.Common;

namespace MongoAPI.Models.Hazards
{
    public abstract class AHazard<T> where T : Enum
    {
        [Range(0, 1)]
        public double Score { get; set; }
        public Level Level { get; set; }
        public Dictionary<T, int> EliminationCosts { get; set; } = new();
        public required bool PreviousIncidentOccurred { get; set; }
        public required string PreviousIncidentDescription { get; set; }
        public required double DistanceToInventory { get; set; }

    }

    public enum FireEliminationMethod
    {
        SmokeDetector,
        FireExtinguisher,
    }

    public enum TsunamiEliminationMethod
    {

    }

    public enum SecurityEliminationMethod
    {

    }

    public enum NoiseEliminationMethod
    {

    }

    public enum AvalancheEliminationMethod
    {

    }

    public enum LandslideEliminationMethod
    {

    }

    public enum RockFallEliminationMethod
    {

    }

    public enum FloodEliminationMethod
    {

    }

    
}
