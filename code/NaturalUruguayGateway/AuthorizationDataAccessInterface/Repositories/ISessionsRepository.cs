using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.AuthorizationDataAccessInterface.Repositories
{
    public interface ISessionsRepository
    {
        Task<User> GetUserAsync(LoginModel login);
        Task<Session> GetSessionAsync(User user);
        Task<Session> CreateSessionAsync(User user);
        Task<int> LogoutAsync(string token);
        Task<Session> GetSessionByToken(string token);
    }
}