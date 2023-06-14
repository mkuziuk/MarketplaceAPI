using System;
using System.Threading.Tasks;
using MarketplaceApi.Models;

namespace MarketplaceApi.IServices
{
    public interface IHelperService
    {
        public Task<User> CheckUser(int userId);
        
        //public Task<(User, Shop)> CheckUserAndShop(int userId, int shopId);
    }
}