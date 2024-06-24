using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<IEnumerable<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
        {
            return await _catalogContext
                         .Products
                         .Find(p => true)
                         .ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _catalogContext
                         .Products
                         .Find(p => p.Id == id)
                         .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _catalogContext
                         .Products
                         .Find(filter)
                         .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrand(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);
            return await _catalogContext
                         .Products
                         .Find(filter)
                         .ToListAsync();
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _catalogContext
                                     .Products
                                     .ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateResult.IsAcknowledged && updateResult.MatchedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _catalogContext
                                                .Products
                                                .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;

        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _catalogContext
                         .Brands
                         .Find(p => true)
                         .ToListAsync();            
        }

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _catalogContext
                       .Types
                       .Find(p => true)
                       .ToListAsync();
        }
    }
}
