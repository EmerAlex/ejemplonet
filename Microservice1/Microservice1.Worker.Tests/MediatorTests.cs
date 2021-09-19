using AutoMapper;
using MediatR;
using Microservice1.Application.Person;
using Microservice1.Application.Ports;
using Microservice1.Domain.Ports;
using Microservice1.Domain.Services;
using Microservice1.Infrastructure;
using Microservice1.Infrastructure.Adapters;
using Microservice1.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Worker.Test
{
    [TestClass]
    public class MediatorTests
    {

        IServiceCollection _services;

        [TestInitialize]
        public void Initialize()
        {
            _services = new ServiceCollection();
            var executingAssembly = typeof(MediatorTests).Assembly;
            _services.AddMediatR(Assembly.Load("Microservice1.Application"), executingAssembly);
            var applicationAssembly = executingAssembly.GetReferencedAssemblies()
                .FirstOrDefault(x => x.Name.Equals("Microservice1.Application", System.StringComparison.InvariantCulture));

            _services.AddAutoMapper(Assembly.Load(applicationAssembly.FullName));
            var JsonConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false).Build();

            _services.AddDbContext<PersistenceContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            _services.AddSingleton<IConfiguration>(c => JsonConfig);
            _services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            _services.AddTransient<IPersonService, PersonService>();
            _services.AddTransient<IRabbitMessaging, MessagingAdapter>();
            _services.AddRabbitSupport(JsonConfig);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .CreateLogger();
        }


        [TestMethod]
        public void TestMethod1()
        {

            var newPerson = new CreatePersonFromMessage("john", "doe", "john@doe.com");

            using (var scope = _services.BuildServiceProvider().CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                var result = mediator.Send(newPerson).Result;
                Assert.IsInstanceOfType(result, typeof(Unit));
            }
        }
    }
}
