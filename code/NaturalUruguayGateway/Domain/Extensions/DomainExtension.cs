using System;
using System.Linq;
using System.Linq.Expressions;

namespace NaturalUruguayGateway.Domain.Extensions
{
    public static class DomainExtension
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> query, string SortField, bool Ascending = false)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { query.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            return query.Provider.CreateQuery<T>(mce);
        }
        
        public static string FirstCharToUpper(this string input)
        {
            string formatted = input;
            if (!string.IsNullOrEmpty(input))
            {
                formatted = input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
            }

            return formatted;
        }
    }
}