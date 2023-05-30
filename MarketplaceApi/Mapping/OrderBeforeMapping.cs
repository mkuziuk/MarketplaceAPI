using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.Views;

namespace MarketplaceApi.Mapping
{
    public class OrderBeforeMapping : IMappingAction<Order, OrderView>
    {
        public void Process(Order source, OrderView destination, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}