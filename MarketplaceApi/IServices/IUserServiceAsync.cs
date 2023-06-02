using System.Threading.Tasks;
using MarketplaceApi.ViewModels;

namespace MarketplaceApi.IServices
{
    public interface IUserServiceAsync
    {
        public Task<UserView> GetUserAsync(int userId);
    }
}