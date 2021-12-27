using Infrastructure.Persistence.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public class HttpHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync("https://localhost:8000", cancellationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Url not responding with 200 OK");
                    }
                }
                catch (Exception)
                {
                    return await Task.FromResult(HealthCheckResult.Unhealthy());
                }
            }
            return await Task.FromResult(HealthCheckResult.Healthy());
        }
    }

    public class SqlServerHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _context;

        public SqlServerHealthCheck(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var connectionString = _context.Database.GetConnectionString();
            await using var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("SQL Unhealthy", ex);
            }
            return HealthCheckResult.Healthy("SQL Healthy");
        }
    }

    //public class ReadisHealthcheck : IHealthCheck
    //{
    //    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
    //    {
    //        using (var redis = ConnectionMultiplexer.Connect("localhost:6379"))
    //        {
    //            try
    //            {
    //                var db = redis.GetDatabase(0);
    //            }
    //            catch (Exception ex)
    //            {
    //                return await Task.FromResult(HealthCheckResult.Unhealthy("Cannot connect to redis cache server.", ex));
    //            }
    //        }
    //        return await Task.FromResult(HealthCheckResult.Healthy("Can connect to redis cache server."));
    //    }
    //}
}