using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.Enums;
using MarketplaceApi.IRepositories;
using MarketplaceApi.IServices;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using MarketplaceApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Services
{
    public class UserServiceAsync : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly DateTime _defaultRegistrationDate = DateTime.Now;

        public UserServiceAsync(IMapper mapper, IUserRepository userRepository)
        {
            //_userRepository = new UserRepository(context);
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserView> GetUserAsync(int userId)
        {
            var userTask = await _userRepository.ExistingUserAsync(userId);
            var userView = _mapper.Map<UserView>(userTask);
            if (userView == null)
                throw new ArgumentNullException($"Пользователь {userId} не существует");
                
            return userView;
        }
    }
}