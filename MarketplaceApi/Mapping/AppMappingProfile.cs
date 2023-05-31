using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.ViewModels;

namespace MarketplaceApi.Mapping
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