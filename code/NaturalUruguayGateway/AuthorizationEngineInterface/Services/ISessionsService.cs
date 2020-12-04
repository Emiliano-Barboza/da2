using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.AuthorizationEngineInterface.Services
{
    public interface ISessionsService
    {
       Task<Session> LoginAsync(LoginModel login);
       Task<string> LogoutAsync(string token);
       Task<Session> GetSessionByTokenAsync(string token);
    }
}