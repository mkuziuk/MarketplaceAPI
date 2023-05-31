using System;
using System.Collections.Generic;
using AutoMapper;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using MarketplaceApi.ViewModels;

namespace MarketplaceApi.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly DateTime _defaultRegistrationDate = DateTime.Now;

        public UserService(MarketplaceContext context, IMapper mapper)
        {
            _userRepository = new UserRepository(context);
            _mapper = mapper;
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<UserView>> GetUser(int userId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<UserView>>
                (StatusCodeEnum.NotFound, new QueryableAndString<UserView>
                    (null, $"Пользователь {userId} не существует"));
            
            var userView = new List<UserView>() {_mapper.Map<UserView>(user)};
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<UserView>>
            (StatusCodeEnum.Ok, new QueryableAndString<UserView>
                (userView, "Получилось")); 
        }

        public KeyValuePair<StatusCodeEnum, string> ChangeInfo
            (int userId, int id, string phone, string email, bool seller)
        {
            var currentUser = _userRepository.ExistingUser(userId);
            if (currentUser == null)
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");
            
            if (userId != id & !currentUser.Admin)
            {
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.NotFound, "У вас нет прав на эту операцию");
            }
            
            var user = _userRepository.ExistingUser(id);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.NotFound, $"Пользователь {id} не существует");
            
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
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");
            
            if (!currentUser.Admin)
            {
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас нет прав на эту операцию");
            }
            
            var user = _userRepository.ExistingUser(id);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {id} не существует");
            
            if (user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {id} уже Администратор");

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
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");
            
            if (userId != id & !currentUser.Admin)
            {
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас нет прав на эту операцию");
            }
            
            var user = _userRepository.ExistingUser(id);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {id} не существует");
            
            _userRepository.Delete(user);
            _userRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось"); 
        }
    }
}