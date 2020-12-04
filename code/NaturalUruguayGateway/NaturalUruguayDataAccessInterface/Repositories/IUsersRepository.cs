using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories
{
    public interface IUsersRepository
    {
        Task<User> AddUserAsync(User userModel);
        Task<User> DeleteUserByIdAsync(int id);
        Task<User> UpdateUserAsync(User userModel);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<PaginatedModel<User>> GetUsersAsync(PagingModel pagingModel);
        Task<UserRole> GetUserRoleByNameAsync(string roleName);
    }
}