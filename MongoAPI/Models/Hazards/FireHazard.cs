﻿namespace MongoAPI.Models.Hazards
{
    public class FireHazard : AHazard<FireEliminationMethod>
    {
        public bool FireSystem { get; set; }
        public bool AfforestationCondition { get; set; }
        public string? ForestType { get; set; }
        public bool StubbleBurning { get; set; }
        public bool ExternalFireIncident { get; set; }
        public string ExternalFireIncidentDescription { get; set; } = string.Empty;
        public bool NearbyGasStation { get; set; }
        public double DistanceToNearbyGasStation { get; set; }
        public bool HasIndustrialFireDanger { get; set; }
        public int IndustrialFireExposedFacade { get; set; }
        public bool ForestFireDanger { get; set; }
        public double DistanceToClosestForest {  get; set; }
        public string VegetationType { get; set; } = string.Empty;
    }
}
