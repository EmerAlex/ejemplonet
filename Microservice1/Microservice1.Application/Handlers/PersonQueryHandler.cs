using AutoMapper;
using MediatR;
using Microservice1.Domain.Ports;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice1.Application.Person
{
    public class PersonQueryHandler : IRequestHandler<PersonQuery, PersonDto>, IDisposable
    {

        private readonly IGenericRepository<Domain.Entities.Person> _PersonRepository;
        private readonly IMapper _Mapper;

        public PersonQueryHandler(IGenericRepository<Domain.Entities.Person> personRepository, IMapper mapper)
        {
            _PersonRepository = personRepository;
            _Mapper = mapper;
        }

        public async Task<PersonDto> Handle(PersonQuery request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException("request", "request object needed to handle this task");
            var response = await _PersonRepository.GetAsync(f => f.Id.Equals(request.Id),
                includeStringProperties: "HomeAddress").ConfigureAwait(false);
            return _Mapper.Map<PersonDto>(response.FirstOrDefault());
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
    }
}
