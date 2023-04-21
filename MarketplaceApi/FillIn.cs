using MarketplaceApi.Models;

namespace MarketplaceApi
{
    public class FillIn
    {
        private Data _data = new Data();
        private readonly MarketplaceContext _context;

        public FillIn(MarketplaceContext context)
        {
            context = _context;
        }

        public void DeleteAll(int rows)
        {
            User user;
            Product product;
            Order order;
            Bill bill;
            OrderedProduct orderedProduct;
            
            
        }
        
        public void FillUser (User user)
        {
            for (int i = 0; i < _data.Id.Length; i++)
            {
                user.Id = _data.Id[i];
                user.Phone = _data.UsersPhones[i];
                user.Email = _data.UsersEmails[i];
                user.FirstName = _data.UsersFirstNames[i];
                user.SecondName = _data.UsersSecondNames[i];
                user.RegistrationDate = _data.UsersRegistartionDate[i];
                user.DeliveryAdress = _data.UsersDeliveryAdresses[i];
                
                _context.Add(user);
            }
        }
    }
}