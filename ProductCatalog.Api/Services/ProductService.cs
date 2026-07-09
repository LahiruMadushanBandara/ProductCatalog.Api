using ProductCatalog.Api.Integrations;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Repositories;

namespace ProductCatalog.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IProductApiClient _client;
        private readonly ILogger<ProductService> _logger;


        public ProductService(IProductRepository repo, IProductApiClient client, ILogger<ProductService> logger)
        {
            _repo = repo;
            _client = client;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var existing = await _repo.GetAllAsync();
            if (existing.Count > 0)
            {
                _logger.LogInformation("Product list from database");
                return existing;
            }
            _logger.LogInformation("No products in database — list from DummyJSON");
            return await _client.FetchAllAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            // check database first
            var existing = await _repo.GetByIdAsync(id);
            if (existing is not null)
            {
                _logger.LogInformation("Product - {Id} is from database", id);
                return existing;
            }

            // fetch from the API
            var fetched = await _client.FetchByIdAsync(id);
            if (fetched is null) return null;

            //Save
            await _repo.InsertAsync(fetched);
            _logger.LogInformation("Product - {Id} is from DummyJson API - not available in Database", id);

            return fetched;
        }
    }
}
