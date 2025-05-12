using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoAPI.Models.KeyModels
{
    public class Client : AEntityBase
    {
        public required string Name { get; set; }
        public ClientType Type { get; set; }
    }

    public enum ClientType
    {
        Private,
        State,
    }
}
