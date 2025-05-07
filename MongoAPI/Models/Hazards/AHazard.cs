using System.ComponentModel.DataAnnotations;

namespace MongoAPI.Models.Hazards
{
    public abstract class AHazard<T> where T : Enum
    {
        [Range(0, 1)]
        public double Score { get; set; }
        public HazardLevel Level { get; set; }
        public Dictionary<T, int> EliminationCosts { get; set; } = new();
        public required bool PreviousIncidentOccurred { get; set; }
        public required string PreviousIncidentDescription { get; set; }
        public required double DistanceToRiskInventory { get; set; }

    }

    public enum FireEliminationMethod
    {
        SmokeDetector,
        FireExtinguisher,
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

    public enum FloodEliminationMethod
    {

    }

    public enum HazardLevel
    {
        VeryLow,
        Low,
        Medium,
        High,
    }
}
