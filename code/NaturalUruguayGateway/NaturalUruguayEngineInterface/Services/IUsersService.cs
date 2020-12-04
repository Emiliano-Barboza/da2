using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayEngineInterface.Services
{
    public interface IUsersService
    {
        Task<User> AddUserAsync(User userModel);
        Task<User> DeleteUserByIdAsync(int id);
        Task<User> UpdateUserAsync(User userModel);
        Task<PaginatedModel<User>> GetUsersAsync(PagingModel pagingModel);
        Task<User> GetUserAsync(int id);
    }
}