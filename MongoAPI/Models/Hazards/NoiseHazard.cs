using MongoAPI.Models.Common;
using MongoAPI.Models.KeyModels;

namespace MongoAPI.Models.Hazards
{
    public class NoiseHazard : AHazard<NoiseEliminationMethod>
    {
        public Dictionary<BuildingType, double> NoiseMeasurementsForBuildings { get; set; } = new();
        public Dictionary<Location, double> NoiseMeasurementsForCoordinates { get; set; } = new();
        public bool ResidentialArea { get; set; }
        public bool Exists { get; set; } // Noise hazard exists
        public bool ExtremeNoise { get; set; }
        public string ExtremeNoiseDescription { get; set; } = string.Empty;
    }
}