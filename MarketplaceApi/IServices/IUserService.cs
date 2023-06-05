using System.Threading.Tasks;
using MarketplaceApi.ViewModels;

namespace MarketplaceApi.IServices
{
    public interface IUserService
    {
        public Task<UserView> GetUserAsync(int userId);
    }
}