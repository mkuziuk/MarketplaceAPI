using System;
using System.Threading.Tasks;
using MarketplaceApi.Exceptions;
using MarketplaceApi.IRepositories;
using MarketplaceApi.IServices;
using MarketplaceApi.Models;

namespace MarketplaceApi.Services
{
    public class HelperService : IHelperService
    {
        private readonly IUserRepository _userRepository;
        private readonly IShopRepository _shopRepository;

        public HelperService(IUserRepository userRepository, IShopRepository shopRepository)
        {
            _userRepository = userRepository;
            _shopRepository = shopRepository;
        }
        
        public async Task<User> CheckUser(int userId)
        {
            var user = await _userRepository.ExistingUserAsync(userId);
            if (user == null)
                throw new NotFoundException($"Пользователь {userId} не существует");

            return user;
        }

        
    }
}