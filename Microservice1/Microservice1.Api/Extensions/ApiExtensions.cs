using Microservice1.Domain.Ports;
using Microservice1.Domain.Services;
using Microservice1.Infrastructure.Adapters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice1.Api.Extensions
{
    public static class ApiExtensions
    {

        public static IServiceCollection LoadAppStoreRepositories(this IServiceCollection services)
        {

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var _services = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.Contains("Domain", StringComparison.InvariantCulture))
                .SelectMany(s => s.GetTypes())
                .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));


            foreach (var _service in _services)
            {
                // interfaces in Domain layer, ignoring interfaces in other namespaces
                var sInterf = _service.GetInterfaces().FirstOrDefault(i => i.FullName.Contains("Domain", StringComparison.InvariantCulture));
                if (sInterf != null)
                {
                    services.AddTransient(sInterf, _service);
                }
            }

            return services;
        }


    }
}
