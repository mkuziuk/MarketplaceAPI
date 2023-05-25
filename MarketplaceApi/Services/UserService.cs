using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;

namespace MarketplaceApi.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        private readonly DateTime _defaultRegistrationDate = DateTime.Now;

        public UserService(MarketplaceContext context)
        {
            _userRepository = new UserRepository(context);
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<User>> GetUser(int userId)
        {
            var user = _userRepository.ExistingUsers(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<User>>
                (StatusCodeEnum.BadRequest, new QueryableAndString<User>
                    (null, $"Пользователь {userId} не существует"));
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<User>>
            (StatusCodeEnum.Ok, new QueryableAndString<User>
                (user, "Получилось")); 
        }

        public KeyValuePair<StatusCodeEnum, string> ChangInfo
            (int userId, int id, string phone, string email, bool seller)
        {
            var currentUser = _userRepository.ExistingUser(userId);
            if (currentUser == null)
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.BadRequest, $"Пользователь {userId} не существует");
            
            if (userId != id & !currentUser.Admin)
            {
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.BadRequest, "У вас нет прав на эту операцию");
            }
            
            var user = _userRepository.ExistingUser(id);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.BadRequest, $"Пользователь {id} не существует");
            
            if (phone != null)
                user.Phone = phone;
            if (email != null)
                user.Email = email;
            user.Seller = seller;
            
            _userRepository.Update(user);
            _userRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось"); 
        }

        public KeyValuePair<StatusCodeEnum, string> ToAdmin(int userId, int id)
        {
            var currentUser = _userRepository.ExistingUser(userId);
            if (currentUser == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {userId} не существует");
            
            if (!currentUser.Admin)
            {
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, "У вас нет прав на эту операцию");
            }
            
            var user = _userRepository.ExistingUser(id);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {id} не существует");
            
            if (user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {id} уже Администратор");

            user.Admin = true;
            
            _userRepository.Update(user);
            _userRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось"); 
        }

        public KeyValuePair<StatusCodeEnum, string> CreateUser(string phone, string email,
            string firstName, string secondName, string deliveryAddress)
        {
            var user = new User()
            {
                Phone = phone,
                Email = email,
                FirstName = firstName,
                SecondName = secondName,
                DeliveryAddress = deliveryAddress,
                RegistrationDate = _defaultRegistrationDate,
                Admin = false,
                Seller = false
            };
            
            _userRepository.Add(user);
            _userRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> DeleteUser(int userId, int id)
        {
            var currentUser = _userRepository.ExistingUser(userId);
            if (currentUser == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {userId} не существует");
            
            if (userId != id & !currentUser.Admin)
            {
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, "У вас нет прав на эту операцию");
            }
            
            var user = _userRepository.ExistingUser(id);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {id} не существует");
            
            _userRepository.Delete(user);
            _userRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось"); 
        }
    }
}