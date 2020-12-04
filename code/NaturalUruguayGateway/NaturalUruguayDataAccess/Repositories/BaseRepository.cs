using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NaturalUruguayGateway.Domain.Extensions;
using NaturalUruguayGateway.Domain.Models;

namespace NaturalUruguayGateway.NaturalUruguayDataAccess.Repositories
{
    public abstract class BaseRepository<T>
    {
        protected readonly DbContext context;
        protected readonly string PrimaryKey = null;

        protected BaseRepository(DbContext context)
        {
            this.context = context;
            this.PrimaryKey = "Id";
        }
        protected async Task<string> GetDefaultPagingOrder(PagingModel pagingModel, string defaultKey)
        {
            await Task.Yield();
            var orderParam = string.IsNullOrEmpty(pagingModel.Order) ? defaultKey : pagingModel.Order;
            orderParam = orderParam.FirstCharToUpper();
            var propertyInfo = typeof(T).GetProperty(orderParam);
            if (propertyInfo == null)
            {
                throw new ArgumentException("Order property not exists");
            }

            return orderParam;
        }
        
        protected async Task<PaginatedModel<T>> GetPaginatedElementsAsync(IQueryable<T> query, PagingModel pagingModel, string defaultKey = "Id")
        {
            PaginatedModel<T> paginated = null;

            if (query == null)
            {
                throw new ArgumentException("Query cannot be null.");
            }
            if (pagingModel == null)
            {
                pagingModel = new PagingModel();
            }

            var orderParam = await GetDefaultPagingOrder(pagingModel, defaultKey);

            var total = query.Count();
            paginated = new  PaginatedModel<T> (pagingModel, total);
            var isAscending = pagingModel.Direction.Equals("asc", StringComparison.InvariantCultureIgnoreCase);
            paginated.Data = query.OrderByField(orderParam, isAscending).Skip(pagingModel.Offset).Take(pagingModel.Limit).ToList();

            return paginated;
        }
    }
}