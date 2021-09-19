using Microservice1.Domain.Entities;
using Microservice1.Domain.Ports;
using Microservice1.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Api.Test
{
    public class IntegrationTestBuilder : IDisposable
    {
        protected HttpClient TestClient;
        private IServiceProvider _serviceProvider;
        private bool Disposed;

        public IEnumerable<Person> People;
        public Guid PersonId;

        protected void BootstrapTestingSuite()
        {
            Disposed = false;
            PersonId = Guid.NewGuid();
            var appFactory = new WebApplicationFactory<Microservice1.Api.Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbCtxOpts = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PersistenceContext>));

                        if (dbCtxOpts != null)
                        {
                            services.Remove(dbCtxOpts);
                        }

                        services.AddDbContext<PersistenceContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });

                        var brokerConn = services.Where(d => d.ServiceType == typeof(IConnection)).ToList();

                        if (brokerConn != null)
                        {
                            foreach (var conn in brokerConn)
                            {
                                services.Remove(conn);
                            }

                        }

                        services.AddSingleton<IConnection>(c =>
                        {
                            var factory = new ConnectionFactory
                            {
                                HostName = "turkey.rmq.cloudamqp.com",
                                UserName = "lzxcpjtp",
                                Password = "OO6voKcyrlTCIZabftP33aKZqKVBT2Vz",
                                Port = 5672,
                                VirtualHost = "lzxcpjtp",
                                AutomaticRecoveryEnabled = true
                            };


                            return factory.CreateConnection("Q_WRITE");


                        });
                    });


                });

            People = new List<Person>
            {
                new Person
                {
                    Email = "JohnDoe@gmail.com", FirstName = "John",
                    LastName = "Doe", Id = PersonId
                }
            };

            _serviceProvider = appFactory.Services;
            SeedDatabase(_serviceProvider);
            TestClient = appFactory.CreateClient();
        }

        void SeedDatabase(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var personRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Person>>();
                foreach (var person in People)
                {
                    personRepo.AddAsync(person).Wait();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                TestClient.Dispose();
            }

            Disposed = true;
        }


    }
}
