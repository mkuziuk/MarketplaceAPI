using AutoMapper;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly MarketplaceContext _context;
        
        protected RepositoryBase(MarketplaceContext context)
        {
            _context = context;
        }
        
        public void Save() => _context.SaveChanges();
    }
}