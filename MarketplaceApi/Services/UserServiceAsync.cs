using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.Enums;
using MarketplaceApi.Exceptions;
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
        private readonly IHelperService _helperService;

        private readonly DateTime _defaultRegistrationDate = DateTime.Now;

        public UserServiceAsync(IMapper mapper, IUserRepository userRepository, IHelperService helperService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _helperService = helperService;
        }

        public async Task<UserView> GetUserAsync(int userId)
        {
            var user = await _helperService.CheckUser(userId);
            var userView = _mapper.Map<UserView>(user);

            return userView;
        }
    }
}