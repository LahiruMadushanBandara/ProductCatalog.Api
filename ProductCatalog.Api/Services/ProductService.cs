using ProductCatalog.Api.Integrations;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Repositories;

namespace ProductCatalog.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IProductApiClient _client;

        public ProductService(IProductRepository repo, IProductApiClient client)
        {
            _repo = repo;
            _client = client;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        } 

        public async Task<Product?> GetByIdAsync(int id)
        {
            // check database first
            var existing = await _repo.GetByIdAsync(id);
            if (existing is not null)
            {
                return existing;
            }

            // fetch from the API
            var fetched = await _client.FetchByIdAsync(id);
            if (fetched is null) return null;

            //Save
            await _repo.InsertAsync(fetched);
            return fetched;
        }
    }
}
