using System;
using System.Threading.Tasks;
using NaturalUruguayGateway.AuthorizationDataAccessInterface.Repositories;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace NaturalUruguayGateway.AuthorizationDataAccess.Repositories
{
    public class SessionsRepository : ISessionsRepository
    {
        private readonly DbSet<User> users;
        private readonly DbSet<Session> sessions;
        private readonly DbContext context;
        
        public SessionsRepository(DbContext context)
        {
            this.context = context;
            this.users = context.Set<User>();
            this.sessions = context.Set<Session>();
        }
        
        public async Task<User> GetUserAsync(LoginModel login)
        {
            try
            {
                var user = await users.FirstOrDefaultAsync(x => x.Email == login.Email && x.Password == login.Password);
                return user;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get user async", e);
            }
        }

        public async Task<Session> GetSessionAsync(User user)
        {
            try
            {
                var session = await sessions.Include("User").FirstOrDefaultAsync(x => x.UserId == user.Id);
                return session;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get session async", e);
            }
        }

        public async  Task<Session> CreateSessionAsync(User user)
        {
            try
            {
                var session = new Session()
                {
                    Token = Guid.NewGuid().ToString(),
                    UserId = user.Id
                };
                sessions.Add(session);
                await context.SaveChangesAsync();
                return session;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on create session async", e);
            }
        }

        public async Task<int> LogoutAsync(string token)
        {
            try
            {
                int result = 0;
                var session = await sessions.FirstOrDefaultAsync(x => x.Token == token);
                if (session != null)
                {
                    sessions.Remove(session);
                    result = await context.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on logout async", e);
            }
        }

        public async Task<Session> GetSessionByToken(string token)
        {
            try
            {
                var session = await sessions.Include(x => x.User.Role)
                    .FirstOrDefaultAsync(x => x.Token == token);
                return session;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get session by token async", e);
            }
        }
    }
}