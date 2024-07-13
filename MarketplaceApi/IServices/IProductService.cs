using System.Threading.Tasks;

namespace MarketplaceApi.IServices
{
    public interface IProductService
    {
        public Task AddProductAsync(ProductEntity productEntity);
    }
}