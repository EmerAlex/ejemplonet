using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice1.Infrastructure.HealthChecks
{
    public class BrokerHealthCheck : IHealthCheck
    {
        private readonly IConnection BrokerConnection;

        public BrokerHealthCheck(IEnumerable<IConnection> connections)
        {
            BrokerConnection = connections.FirstOrDefault(c => c.ClientProvidedName.Contains("READ", StringComparison.InvariantCulture)) ??
                throw new ArgumentNullException("connections", "Missing dependency: broker connection");
        }


        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            if (BrokerConnection.IsOpen)
            {
                return await Task.FromResult(HealthCheckResult.Healthy("A healthy result.")).ConfigureAwait(false);
            }

            return await Task.FromResult(HealthCheckResult.Unhealthy("An unhealthy result.")).ConfigureAwait(false);

        }
    }
}
