using Microservice1.Domain.Entities;
using Microservice1.Domain.Ports;
using Microservice1.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microservice1.Domain.Tests
{
    [TestClass]
    public class PersonServiceTest
    {

        IGenericRepository<Person> PersonRepository;
        PersonService _PersonService;
        Guid PersonId;

        [TestInitialize]
        public void Initialize()
        {
            PersonRepository = NSubstitute.Substitute.For<IGenericRepository<Person>>();
            var ilogger = NSubstitute.Substitute.For<ILogger<PersonService>>();
            _PersonService = new PersonService(PersonRepository);
            PersonId = Guid.NewGuid();


        }

        [TestMethod]
        public void FindPersonTest()
        {

            PersonRepository.GetAsync(Arg.Any<Expression<Func<Person, bool>>>(),
               Arg.Any<Func<IQueryable<Person>,
               IOrderedQueryable<Person>>>(), includeObjectProperties: Arg.Any<Expression<Func<Person, object>>>())
                .Returns(new List<Person> {
                   new Person {
                       Email = "user@server.net",
                       FirstName = "John", HomeAddress = new List<Address> {
                           new Address("Calle Q, Lote grande", "20")
                       },
                       LastName = "Doe", Id = PersonId
                   }
               });

            var result = _PersonService.FindPersonAsync(p => p.Id.Equals(PersonId)).Result;

            Assert.IsTrue(result.FirstOrDefault().Id.Equals(PersonId));
        }


    }
}
