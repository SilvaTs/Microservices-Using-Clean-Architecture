using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities
{
    public class Product : BaseEntity
    {
        [BsonRepresentation("Name")]
        public string Name { get; set; }
        public string Sumary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public ProductBrand Brands { get; set; }
        public ProductType Types { get; set; }
        public decimal Price { get; set; }
    }
}
