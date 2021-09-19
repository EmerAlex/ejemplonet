using Microservice1.Application.Ports;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice1.Infrastructure.Adapters
{
    public class MessagingAdapter : IRabbitMessaging
    {
        private readonly IConnection BrokerConnection;

        public MessagingAdapter(IEnumerable<IConnection> conn)
        {
            BrokerConnection = conn.FirstOrDefault(f => f.ClientProvidedName.Contains("WRITE", StringComparison.InvariantCulture));
        }


        public async Task SendMessageAsync(object o, string queue)
        {
            using (var channel = BrokerConnection.CreateModel())
            {
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var message = System.Text.Json.JsonSerializer.Serialize(o);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();

                properties.CorrelationId = Guid.NewGuid().ToString();

                properties.Persistent = true;

                await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: properties, body: body)).ConfigureAwait(false);
            }
        }


    }
}
