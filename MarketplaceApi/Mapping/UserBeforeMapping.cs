using System;
using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.Views;

namespace MarketplaceApi.Mapping
{
    public class UserBeforeMapping : IMappingAction<User, UserView>
    {
        public void Process(User source, UserView destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}