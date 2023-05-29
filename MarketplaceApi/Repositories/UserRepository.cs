using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Entities;
using MarketplaceApi.Models;
using MarketplaceApi.Views;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Repositories
{
    public class UserRepository : Repository
    {
        public UserRepository(MarketplaceContext context) : base(context) {}

        private IEnumerable<UserView> SelectUserView() => Context.User
            .Select(u => new UserView()
            {
                Id = u.Id,
                Phone = u.Phone,
                Email = u.Email,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                RegistrationDate = u.RegistrationDate,
                DeliveryAddress = u.DeliveryAddress,
                Seller = u.Seller
            });

        public User ExistingUser(int userId) => Context.User.FirstOrDefault(u => u.Id == userId);
        
        public IEnumerable<UserView> ExistingUsersView(int userId) => SelectUserView()
            .Where(u => u.Id == userId);
        
        public IQueryable<User> ExistingUsers(int userId) => Context.User.Where(u => u.Id == userId);

        public IEnumerable<UserView> ExistingUsers(IEnumerable<int> userIds)
        {
            var users = Context.User;
            return users
                .Select(u => new UserView()
                {
                    Id = u.Id,
                    Phone = u.Phone,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    SecondName = u.SecondName,
                    RegistrationDate = u.RegistrationDate,
                    DeliveryAddress = u.DeliveryAddress,
                    Seller = u.Seller
                })
                .Where(u => userIds.Contains(u.Id));
        }


        public User UserByOrderId(int orderId) => Context.User
            .FirstOrDefault(u => u.Orders.Any(o => o.Id == orderId));
        
        public bool IsModeratorInShop(int userId, int shopId) => Context.User
            .Any(u => u.ShopsWhereModerator
                .FirstOrDefault(swm => swm.Moderators
                    .Any(m => m.Id == userId)).Id == shopId);

        public void Update(User user) => Context.User.Update(user);
        public void Add(User user) => Context.User.Add(user);
        public void Attach(User user) => Context.User.Attach(user);
        public void Delete(User user) => Context.User.Remove(user);
    }
}