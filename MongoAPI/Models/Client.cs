namespace MongoAPI.Models
{
    public class Client
    {
        //public int ID { get; set; } // Foreign Keys are not needed
        public required string Name { get; set; }
        public ClientType Type { get; set; }
        public required List<Region> Regions { get; set; }
    }

    public enum ClientType
    {
        Private,
        State,
    }
}
