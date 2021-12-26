using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Application.Commons.Extensions
{
    public static class EfExtensions
    {
        /// <summary>
        /// Execute a query asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> QueryAsync<T>(this DbContext dbContext, string spName, object parameter, CommandType commandType = CommandType.StoredProcedure) where T : class
        {
            using (var cn = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
            {
                await cn.OpenAsync();
                return await cn.QueryAsync<T>(spName, parameter, commandType: commandType);
            }
        }

        /// <summary>
        /// Execute a query asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static async Task<T> QueryFirstOrDefaultAsync<T>(this DbContext dbContext, string sql, object parameter = null) where T : class
        {
            using (var cn = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
            {
                await cn.OpenAsync();
                return await cn.QueryFirstOrDefaultAsync<T>(sql, parameter);
            }
        }

        /// <summary>
        /// Execute paramterized SQL that selects a single value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteScalarAsync<T>(this DbContext dbContext, string spName, object parameter, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var cn = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
            {
                await cn.OpenAsync();
                return await cn.ExecuteScalarAsync<T>(spName, parameter, commandType: commandType);
            }
        }

        /// <summary>
        /// Execute a command asynchronously
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteAsync(this DbContext dbContext, string spName, object parameter, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var cn = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
            {
                await cn.OpenAsync();
                return await cn.ExecuteAsync(spName, parameter, commandType: commandType);
            }
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<(IList<T1> firstSet, IList<T2> secondSet)> QueryMultiple<T1, T2>(this DbContext dbContext, string spName, object parameter, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();

                using (var multi = connection.QueryMultiple(spName, parameter, commandType: commandType))
                {
                    var result1 = (await multi.ReadAsync<T1>()).ToList();
                    var result2 = (await multi.ReadAsync<T2>()).ToList();
                    return (result1, result2);
                }
            }
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="spName"></param>
        /// <param name="parameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<(IList<T1>, IList<T2>, IList<T3>)> QueryMultiple<T1, T2, T3>(this DbContext dbContext, string spName, object parameter, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
            {
                await connection.OpenAsync();

                using (var multi = connection.QueryMultiple(spName, parameter, commandType: commandType))
                {
                    var result1 = (await multi.ReadAsync<T1>()).ToList();
                    var result2 = (await multi.ReadAsync<T2>()).ToList();
                    var result3 = (await multi.ReadAsync<T3>()).ToList();
                    return (result1, result2, result3);
                }
            }
        }
    }
}