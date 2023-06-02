using System.Threading.Tasks;

namespace MarketplaceApi.IServices
{
    public interface IProductServiceAsync
    {
        public Task AddProductAsync(ProductEntity productEntity);
    }
}