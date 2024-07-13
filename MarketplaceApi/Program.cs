using System.Text.Json;
using System.Text.Json.Serialization;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MarketplaceApi
{
    public class Program
    {
        //private MarketplaceContext _context;
        //private static UserRepository _userRepository;
        //private static ShopRepository _shopRepository;
        
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            CreateHostBuilder(args).Build().Run();

            //AddModeratorToShop(1, 2);
        }
/*
        private Program(MarketplaceContext context)
        {
            _context = context;
            _userRepository = new UserRepository(context);
            _shopRepository = new ShopRepository(context);
        }

        private static void AddModeratorToShop(int shopId, int newModeratorId)
        {
            var shop = _shopRepository.ExistingShop(shopId);
            
            var newModerator = _userRepository.ExistingUser(newModeratorId);
            
            shop.Moderators.Add(newModerator);
            
            _shopRepository.Update(shop);
            _shopRepository.Save();
        }
        */

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}