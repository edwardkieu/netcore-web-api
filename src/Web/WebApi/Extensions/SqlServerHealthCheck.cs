using Infrastructure.Persistence.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
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
}