using Microservice1.Domain.Entities;
using Microservice1.Domain.Ports;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservice1.Domain.Services
{
    [DomainService]
    public class PersonService : IPersonService, IDisposable
    {
        private readonly IGenericRepository<Person> _PersonRepository;

        public PersonService(IGenericRepository<Person> personRepository)
        {
            _PersonRepository = personRepository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            this._PersonRepository.Dispose();
        }

        public async Task<IEnumerable<Person>> FindPersonAsync(Expression<Func<Person, bool>> filter)
        {
            return await _PersonRepository.GetAsync(filter, includeObjectProperties: x => x.HomeAddress).ConfigureAwait(false);
        }

        public async Task<Person> SavePersonAsync(Person p)
        {
            return await _PersonRepository.AddAsync(p).ConfigureAwait(false);
        }

    }
}

