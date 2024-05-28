using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication2.Models
{
    public class ProductList
    {
        [BsonElement("product")]
        public string Product { get; set; }

        [BsonElement("amount")]
        public string Amount { get; set; }
    }
}
