using AutoMapper;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly MarketplaceContext Context;
        
        protected RepositoryBase(MarketplaceContext context)
        {
            Context = context;
        }
        
        public void Save() => Context.SaveChanges();
    }
}