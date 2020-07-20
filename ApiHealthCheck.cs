using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace a_web_api
{
    public class ApiHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            //TODO: Implement your own healthcheck logic here
            var isHealthy = true;
            if (isHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("I am one healthy microservice API"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("I am the sad, unhealthy microservice API"));
        }
    }
}
