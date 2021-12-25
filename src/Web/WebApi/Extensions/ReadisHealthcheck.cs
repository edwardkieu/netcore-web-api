using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.Extensions
{
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