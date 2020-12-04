using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Configuration;
using NaturalUruguayGateway.HelperInterface.Services;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;

namespace NaturalUruguayGateway.NaturalUruguayEngine.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository repository;
        private readonly IConfigurationManager configuration;
        private readonly IEncryptor encryptor;
        private readonly string adminRole;
        
        public UsersService(IUsersRepository repository, IConfigurationManager configuration, IEncryptor encryptor)
        {
            this.repository = repository;
            this.configuration = configuration;
            this.encryptor = encryptor;
            this.adminRole = "Admin";
        }
        
        public async Task<User> AddUserAsync(User userModel)
        {
            var user = await repository.GetUserByEmailAsync(userModel.Email);
            if (user != null)
            {
                throw new DuplicateNameException("email already exists in the system.");
            }
            var userRole = await repository.GetUserRoleByNameAsync(adminRole);
            if (userRole == null)
            {
                throw new KeyNotFoundException("The admin role was not configured.");
            }
            userModel.Password = await encryptor.EncryptAsync(configuration.DefaultPassword);
            userModel.RoleId = userRole.Id;
            user = await repository.AddUserAsync(userModel);
            return user;
        }

        public async Task<User> DeleteUserByIdAsync(int id)
        {
            var user = await repository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("The user doesn't exists.");
            }
            user = await repository.DeleteUserByIdAsync(id);
            return user;
        }

        public async Task<User> UpdateUserAsync(User userModel)
        {
            var user = await repository.GetUserByIdAsync(userModel.Id);
            if (user == null)
            {
                throw new KeyNotFoundException("The user doesn't exists");
            }
            if (!string.IsNullOrWhiteSpace(userModel.Email))
            {
                var userEmail = await repository.GetUserByEmailAsync(userModel.Email);
                if (userEmail != null && userEmail.Id != user.Id)
                {
                    throw new DuplicateNameException("email already exists in the system.");
                }
            }
            if (!string.IsNullOrWhiteSpace(userModel.Password))
            {
                userModel.Password = await encryptor.EncryptAsync(userModel.Password);
            }
            
            user = await repository.UpdateUserAsync(userModel);
            return user;
        }

        public async Task<PaginatedModel<User>> GetUsersAsync(PagingModel pagingModel)
        {
            PaginatedModel<User> paginatedUsers = await repository.GetUsersAsync(pagingModel);
            return paginatedUsers;
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await repository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }
            return user;
        }
    }
}