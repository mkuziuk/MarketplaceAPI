using System;
using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.ViewModels;

namespace MarketplaceApi.Mapping
{
    public class ProductBeforeMapping : IMappingAction<Product, ProductView>
    {
        public void Process(Product source, ProductView destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}