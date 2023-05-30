using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.Views;

namespace MarketplaceApi
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Product, ProductView>();
            CreateMap<User, UserView>();
            CreateMap<Order, OrderView>();
        }
    }
}