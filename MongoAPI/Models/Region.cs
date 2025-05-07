namespace MongoAPI.Models
{
    public class Region
    {
        public required int Id { get; set; }  // Replaced with No (genel_bilgi: bolge_no)
        public required List<string> Cities { get; set; }
        public required string Headquarters { get; set; }
        public required List<TM> TMList { get; set; }
    }
}
