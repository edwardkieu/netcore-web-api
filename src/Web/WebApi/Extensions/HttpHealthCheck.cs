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
}