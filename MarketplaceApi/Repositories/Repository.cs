using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public abstract class Repository
    {
        protected readonly MarketplaceContext Context;

        protected Repository(MarketplaceContext context)
        {
            Context = context;
        }

        public void Save() => Context.SaveChanges();
    }
}