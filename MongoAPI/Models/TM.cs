namespace MongoAPI.Models
{
    public class TM
    {
        public int Id { get; set; }
        public List<TM>? Alternatives { get; set; }
        public required string Name { get; set; }
        public TMState State { get; set; } = TMState.Active;
        public required List<int> Voltages { get; set; }
        public int MaxVoltage { get => Voltages.Max(); }
        public DateOnly ProvisionalAcceptanceDate { get; set; } = new DateOnly();
        public required Location Location { get; set; }
        public string City { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public required EarthquakeLevel DD1 { get; set; }
        public required EarthquakeLevel DD2 { get; set; }
        public required EarthquakeLevel DD3 { get; set; }
        public required EarthquakeLevel EarthquakeScenario { get; set; }
        public required List<Building> Buildings { get; set; } = new();
    }

    public enum TMType
    {
        GIS,
        Default
    }

    public enum TMState
    {
        Active,
        Inactive,
    }
}
