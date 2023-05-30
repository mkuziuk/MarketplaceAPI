using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.Views;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi
{
    public static class QueryableExtensions
    {
        public static IQueryable<ProductView> SelectProductView(this IQueryable<Product> collection) => 
            collection.Select(p => new ProductView()
            {
                Id = p.Id, 
                UserId = p.UserId,
                Name = p.Name, 
                ShopId = p.ShopId, 
                Price = p.Price, 
                Material = p.Material, 
                Type = p.Type, 
                UseCase = p.UseCase,
                WhereUsed = p.WhereUsed,
                Length = p.Length,
                Width = p.Width,
                Height = p.Height,
                InStockQuantity = p.InStockQuantity,
                IsPublic = p.IsPublic,
                PublicationDate = p.PublicationDate
            });

        /*
        public static IEnumerable<T1> SelectView<T1, T2>(this IQueryable<T2> collection)
        {
            var pT1 = typeof(T1).GetProperties();

            var pT2 = typeof(T2).GetProperties();
            
            return collection.Select(c => new T1()
            {
                
            });
        }
        */
    }
}