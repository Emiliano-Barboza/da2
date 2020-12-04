using System.Security.Authentication;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.AuthorizationDataAccessInterface.Repositories;
using NaturalUruguayGateway.AuthorizationEngineInterface.Services;
using NaturalUruguayGateway.HelperInterface.Configuration;
using NaturalUruguayGateway.HelperInterface.Services;

namespace NaturalUruguayGateway.AuthorizationEngine.Services
{
    public class SessionsService : ISessionsService
    {
        private readonly ISessionsRepository repository;
        private readonly IConfigurationManager configuration;
        private readonly IEncryptor encryptor;
        
        public SessionsService(ISessionsRepository repository, IConfigurationManager configuration, IEncryptor encryptor)
        {
            this.repository = repository;
            this.configuration = configuration;
            this.encryptor = encryptor;
        }
        
        public async Task<Session> LoginAsync(LoginModel login)
        {
            login.Password = await encryptor.EncryptAsync(login.Password);
            var user = await repository.GetUserAsync(login);
            if (user == null)
            {
                throw new InvalidCredentialException("Invalid credentials");
            }
            var session = await repository.GetSessionAsync(user);
            if (session == null)
            {
                session = await repository.CreateSessionAsync(user);
            }
            return session;
        }

        public async Task<string> LogoutAsync(string token)
        {
            string logoutUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(token))
            {
                var schema = configuration.AuthenticationSchema;
                var rawToken = token.Replace(schema, "");
                await repository.LogoutAsync(rawToken);
                logoutUrl = configuration.LogoutRedirectUrl; 
            }
            else
            {
                throw  new InvalidCredentialException();
            }
            
            return logoutUrl;
        }

        public async Task<Session> GetSessionByTokenAsync(string token)
        {
            var session = await repository.GetSessionByToken(token);
            return session;
        }
    }
}