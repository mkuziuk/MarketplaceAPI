using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using MarketplaceApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Services
{
    public class UserServiceAsync
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly DateTime _defaultRegistrationDate = DateTime.Now;

        public UserServiceAsync(MarketplaceContext context, IMapper mapper)
        {
            _userRepository = new UserRepository(context);
            _mapper = mapper;
        }

        public async Task<UserView> GetUserAsync(int userId)
        {
            var userTask = await _userRepository.ExistingUserAsync(userId);
            var userView = _mapper.Map<UserView>(userTask);
            if (userView == null)
                throw new ArgumentNullException($"User {userId} doesn't exist");
                
            return userView;
        }
    }
}