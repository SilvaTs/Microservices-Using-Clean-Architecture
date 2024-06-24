using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            bool checkBrands = brandCollection.Find(b => true).Any();
            string path = @"C:\cleanArchitecture\Microservices-Using-Clean-Architecture\Services\Catalog\Catalog.Infrastructure\Data\SeedData\brands.json";
            if (!checkBrands)
            {
                var brandsData = File.ReadAllText(path);
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands is not null)
                {
                    foreach (var item in brands)
                    {
                        brandCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
