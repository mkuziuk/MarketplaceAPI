using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.Views;

namespace MarketplaceApi.Mapping
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Product, ProductView>()
                .BeforeMap<ProductBeforeMapping>();
            CreateMap<User, UserView>()
                .BeforeMap<UserBeforeMapping>();
            CreateMap<Order, OrderView>()
                .BeforeMap<OrderBeforeMapping>();
        }
    }
}