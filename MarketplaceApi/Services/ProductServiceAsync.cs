using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.Exceptions;
using MarketplaceApi.IRepositories;
using MarketplaceApi.IServices;
using MarketplaceApi.Models;

namespace MarketplaceApi.Services
{
    public class ProductServiceAsync : IProductService
    {
        private const decimal PriceFluctuation = 0.1M;
        private const decimal SizeFluctuation = 0.1M;
        private const decimal VolumeFluctuation = 0.1M;

        private readonly IUserRepository _userRepository;

        //private readonly ProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IHelperService _helperService;

        public ProductServiceAsync(IMapper mapper, IProductRepository productRepository, IUserRepository userRepository,
            IShopRepository shopRepository, IHelperService helperService)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _shopRepository = shopRepository;
            _mapper = mapper;
            _helperService = helperService;
        }

        public async Task AddProductAsync(ProductEntity productEntity)
        {
            var user = (await CheckUserAndShop(productEntity.UserId, productEntity.ShopId)).Item1;

            await CheckRights(productEntity.UserId, productEntity.ShopId, user);

            IfTypeExists(productEntity.Type);
            IfUseCaseExists(productEntity.UseCase);
            IfWhereUsedExists(productEntity.WhereUsed);
            IfMaterialExists(productEntity.Material);

            var product = new Product()
            {
                UserId = productEntity.UserId,
                Name = productEntity.Name,
                Type = productEntity.Type,
                UseCase = productEntity.UseCase,
                WhereUsed = productEntity.WhereUsed,
                Material = productEntity.Material,
                Length = productEntity.Length,
                Width = productEntity.Width,
                Height = productEntity.Height,
                Price = productEntity.Price,
                InStockQuantity = productEntity.Quantity,
                CreationDate = DateTime.Now,
                ShopId = productEntity.ShopId,
                IsPublic = productEntity.IsPublic
            };

            if (productEntity.IsPublic) product.PublicationDate = DateTime.Now;

            _productRepository.Add(product);
            _productRepository.Save();
        }
        
        private async Task<(User, Shop)> CheckUserAndShop(int userId, int shopId)
        {
            var userTask = _userRepository.ExistingUserAsync(userId);
            var shopTask = _shopRepository.ExistingShopAsync(shopId);
                
            if (await userTask == null)
                throw new NotFoundException($"Пользователь {userId} не существует");
            if (await shopTask == null)
                throw new NotFoundException($"Магазин {shopId} не существует");

            return (userTask.Result, shopTask.Result);
        }

        private async Task CheckRights(int userId, int shopId, User user)
        {
            var isModeratorInShop = await _userRepository.IsModeratorInShopAsync(userId, shopId);
            if (!isModeratorInShop & !user.Admin)
                throw new AccessException("У вас нет прав на добавление товаров в этом магазине");
        }

        private static void IfTypeExists(int type)
        {
            var ifTypeExists = ProductEntity.ListOfTypes.Any(lt => lt.Equals(type));
            if (!ifTypeExists)
                throw new InvalidDataException($"Тип {type} не существует");
        }
        
        private static void IfUseCaseExists(int useCase)
        {
            var ifUseCaseExists = ProductEntity.ListOfUseCases.Any(uc => uc.Equals(useCase));
            if (!ifUseCaseExists)
                throw new InvalidDataException($"Способ применения {useCase} не существует");
        }
        
        private static void IfWhereUsedExists(int whereUsed)
        {
            var ifWhereUsedExists = ProductEntity.ListOfWhereUsed
                .Any(wu => wu.Equals(whereUsed));
            if (!ifWhereUsedExists)
                throw new InvalidDataException($"Место применения {whereUsed} не существует");
        }
        
        private static void IfMaterialExists(int material)
        {
            var ifMaterialExists = ProductEntity.ListOfMaterials.Any(lm => lm.Equals(material));
            if (!ifMaterialExists)
                throw new InvalidDataException($"Материал {material} не существует");
        }
    }
}