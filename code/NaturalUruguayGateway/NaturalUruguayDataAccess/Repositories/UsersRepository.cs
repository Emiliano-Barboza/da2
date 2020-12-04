using System;
using System.Linq;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain;
using NaturalUruguayGateway.Domain.Exceptions;
using NaturalUruguayGateway.Domain.Models;
using Microsoft.EntityFrameworkCore;
using NaturalUruguayGateway.NaturalUruguayDataAccessInterface.Repositories;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        private readonly DbSet<User> users;
        private readonly DbSet<UserRole> usersRole;
        
        public UsersRepository(DbContext context) : base(context)
        {
            this.users = context.Set<User>();
            this.usersRole = context.Set<UserRole>();
        }
        public async Task<User> AddUserAsync(User userModel)
        {
            try
            {
                var user = users.Add(userModel);
                await context.SaveChangesAsync();
                return user.Entity;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on add user async", e);
            }
        }

        public async Task<User> DeleteUserByIdAsync(int id)
        {
            try
            {
                var user = await users.FirstOrDefaultAsync(x => x.Id == id);
                user.IsDeleted = true;
                await context.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on delete user async", e);
            }
        }

        public async Task<User> UpdateUserAsync(User userModel)
        {
            try
            {
                var user = await users.FirstOrDefaultAsync(x => x.Id == userModel.Id);

                user.Email = userModel.Email != null && user.Email != userModel.Email ? userModel.Email : user.Email; 
                user.Name = userModel.Name != null && user.Name != userModel.Name ? userModel.Name : user.Name; 
                user.Password = userModel.Password != null && user.Password != userModel.Password ? userModel.Password : user.Password; 
            
                await context.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on update user async", e);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await users.FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
                return user;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get user by email async", e);
            }
        }
        
        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
                return user;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get user by id async", e);
            }
        }

        public async Task<PaginatedModel<User>> GetUsersAsync(PagingModel pagingModel)
        {
            try
            {
                await Task.Yield();
                IQueryable<User> query = users.Where(x => !x.IsDeleted).AsQueryable();
                if (!string.IsNullOrEmpty(pagingModel.FilterBy))
                {
                    query = query.Where(
                        x => x.Name.ToUpper().Contains(pagingModel.FilterBy.ToUpper()));
                }
                var paginated = await GetPaginatedElementsAsync(query, pagingModel, PrimaryKey);
                return paginated;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get users async", e);
            }
        }

        public async Task<UserRole> GetUserRoleByNameAsync(string roleName)
        {
            try
            {
                var userRole = await usersRole.FirstOrDefaultAsync(x => x.Name == roleName);
                return userRole;
            }
            catch (Exception e)
            {
                throw new WrappedDbException("Error on get user role by name async", e);
            }
        }
    }
}