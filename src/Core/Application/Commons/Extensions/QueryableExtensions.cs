using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Application.Commons.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<int> MaxOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, int?>> selector, int nullValue = 0)
        {
            return await source.MaxAsync(selector) ?? nullValue;
        }
        
        public static (IQueryable<T> query, int totalCount) ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var totalCount = query.Count();
            var collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return (collection, totalCount);
        }

        /// <summary>
        /// EntityName=titan(future: EntityName:contain=titan)
        /// SortBy=CreatedDate:asc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="uriQuery"></param>
        /// <param name="obj">IQueryable</param>
        /// <returns></returns>
        public static IQueryable<T> AddQuery<T>(this IQueryable<T> query, Dictionary<string, string> uriQuery, object obj)
        {
            //IQueryable<LegalHold> query = legalHolds.AsQueryable();
            //var uirQuery = new Dictionary<string, string>()
            //{
            //    { "Id", Guid.NewGuid().ToString()},
            //    { "Name", "name" },
            //    { "Status", "123" },
            //    { "SortBy", "Name:asc" }
            //};

            //_userCatalogService.AddQuery(query, uirQuery, new GetLegalHoldPackageResult());
            foreach (var data in uriQuery)
            {
                var proper = obj.GetType().GetProperty(data.Key);
                if (proper != null)
                {
                    var type = proper.PropertyType;
                    if (type == typeof(string))
                    {
                        query = query.Where(x => x.GetType().GetProperty(data.Key).GetValue(x) != null && x.GetType().GetProperty(data.Key).GetValue(x).ToString().ToLower().Contains(data.Value.ToLower()));
                    }
                    if (type == typeof(Guid) && Guid.TryParse(data.Value, out Guid id))
                    {
                        query = query.Where(x => new Guid(x.GetType().GetProperty(data.Key).GetValue(x).ToString()) == id);
                    }
                    if (type == typeof(Int64) && Int64.TryParse(data.Value, out long number))
                    {
                        query = query.Where(x => Int64.Parse(x.GetType().GetProperty(data.Key).GetValue(x).ToString()) == number);
                    }
                }
            }

            var sortBy = uriQuery.GetValueOrDefault("SortBy");
            if (!string.IsNullOrEmpty(sortBy))
            {
                var arr = sortBy.Split(":");
                if (obj.GetType().GetProperty(arr[0]) == null)
                {
                    return query;
                }
                if (arr.Length == 2)
                {
                    if (arr[1] == "asc")
                    {
                        query = query.OrderBy(x => x.GetType().GetProperty(arr[0]).GetValue(x));
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.GetType().GetProperty(arr[0]).GetValue(x));
                    }
                }
                else
                {
                    query = query.OrderBy(x => x.GetType().GetProperty(arr[0]).GetValue(x));
                }
            }
            return query;
        }

        //Inspired by https://stackoverflow.com/a/31959568/5274

        static readonly MethodInfo SortOrderBy = typeof(Queryable).GetMethods()
            .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition && m.GetParameters().Length == 2)
            .Single();

        static readonly MethodInfo SortOrderByDescending = typeof(Queryable).GetMethods()
            .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition && m.GetParameters().Length == 2)
            .Single();

        static readonly MethodInfo SortThenBy = typeof(Queryable).GetMethods()
            .Where(m => m.Name == "ThenBy" && m.IsGenericMethodDefinition && m.GetParameters().Length == 2)
            .Single();

        static readonly MethodInfo SortThenByDescending = typeof(Queryable).GetMethods()
            .Where(m => m.Name == "ThenByDescending" && m.IsGenericMethodDefinition && m.GetParameters().Length == 2)
            .Single();

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            return BuildQuery(SortOrderBy, query, propertyName);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName, bool isDescending = false)
        {
            return isDescending ? BuildQuery(SortOrderByDescending, query, propertyName) : BuildQuery(SortOrderBy, query, propertyName);
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IQueryable<TSource> query, string propertyName, bool isDescending = false)
        {
            return isDescending ? BuildQuery(SortThenByDescending, query, propertyName) : BuildQuery(SortThenBy, query, propertyName);
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            return BuildQuery(SortOrderByDescending, query, propertyName);
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            return BuildQuery(SortThenBy, query, propertyName);
        }

        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            return BuildQuery(SortThenByDescending, query, propertyName);
        }

        private static IOrderedQueryable<TSource> BuildQuery<TSource>(MethodInfo method, IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo == null)
                throw new ArgumentOutOfRangeException(nameof(propertyName), "Unknown column " + propertyName);

            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            return (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector })!;
        }
    }
}