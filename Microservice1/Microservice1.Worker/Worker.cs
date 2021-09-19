using MediatR;
using Microservice1.Application.Person;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice1.Worker
{
    //we can have many of this background service if needed
    public sealed class Worker : BackgroundService, IDisposable
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConnection _readBrokerConnection;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;
        ManualResetEvent _quitEvent;

        // let's inject a service provider to create scopes
        public Worker(IServiceProvider services, IEnumerable<IConnection> brokerConnection)
        {
            _services = services ?? throw new ArgumentNullException("services", "Service provider required to build scopes");
            _logger = _services.GetService(typeof(ILogger<Worker>)) as ILogger<Worker>;
            _config = _services.GetService(typeof(IConfiguration)) as IConfiguration;
            _readBrokerConnection = brokerConnection != null
                ? brokerConnection.FirstOrDefault(b => b.ClientProvidedName.Contains("READ", StringComparison.InvariantCulture))
                : throw new ArgumentNullException("brokerConnection", "No brokers available");

        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueName = _config.GetValue<string>("QueueName");
            _quitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eArgs) =>
            {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };

            using (var channel = _readBrokerConnection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                channel.ConfirmSelect();

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, e) =>
                {
                    try
                    {
                        using (var scope = _services.CreateScope())
                        {
                            var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            var createPersonRequest = System.Text.Json.JsonSerializer.Deserialize<CreatePersonCommandAsync>(System.Text.Encoding.Default.GetString(e.Body.ToArray()));
                            // here we could've use a mapper, keep in mind
                            _ = await _mediator.Send(new CreatePersonFromMessage(createPersonRequest.FirstName,
                                createPersonRequest.LastName, createPersonRequest.Email));
                            channel.BasicAck(e.DeliveryTag, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"some error : {ex.Message}");
                        channel.BasicNack(e.DeliveryTag, false, false);
                    }
                };

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                await Task.Run(() =>
                {
                    _quitEvent.WaitOne();
                }, stoppingToken);

            }
            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            _quitEvent.Dispose();
            base.Dispose();
        }

    }
}
