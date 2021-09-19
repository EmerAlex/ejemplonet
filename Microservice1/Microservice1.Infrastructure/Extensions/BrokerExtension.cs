
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Microservice1.Infrastructure.Extensions
{
    public static class BrokerExtension
    {
        public static IServiceCollection AddRabbitSupport(this IServiceCollection services, IConfiguration config)
        {

            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITHOST") ?? config.GetValue<string>("RABBITMQ:HOST"),
                UserName = Environment.GetEnvironmentVariable("RABBITUSER") ?? config.GetValue<string>("RABBITMQ:USER"),
                Password = Environment.GetEnvironmentVariable("RABBITPASS") ?? config.GetValue<string>("RABBITMQ:PASS"),
                Port = Environment.GetEnvironmentVariable("RABBITPORT") != null ?
                        Int32.Parse(Environment.GetEnvironmentVariable("RABBITPORT"), CultureInfo.InvariantCulture) : config.GetValue<int>("RABBITMQ:PORT"),
                VirtualHost = Environment.GetEnvironmentVariable("RABBITVHOST") ?? config.GetValue<string>("RABBITMQ:VHOST"),
                AutomaticRecoveryEnabled = true
            };

            services.AddSingleton<IConnection>((svc) =>
            {
                return factory.CreateConnection($"{config.GetValue<string>("AppName") ?? "MyApp"}_WRITE");
            });

            services.AddSingleton<IConnection>((svc) =>
            {
                return factory.CreateConnection($"{config.GetValue<string>("AppName") ?? "MyApp"}_READ");
            });

            return services;
        }

    }
}
