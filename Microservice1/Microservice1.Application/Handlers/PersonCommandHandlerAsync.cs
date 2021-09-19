using MediatR;
using Microservice1.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice1.Application.Person
{
    public class PersonCommandHandlerAsync : AsyncRequestHandler<CreatePersonCommandAsync>
    {

        private readonly IPersonService _PersonService;

        public PersonCommandHandlerAsync(IPersonService personService)
        {
            _PersonService = personService;
        }

        protected override async Task Handle(CreatePersonCommandAsync request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException("request", "request object needed to handle this task");
            await _PersonService.SavePersonAsync(
                 new Domain.Entities.Person
                 {
                     Email = request.Email,
                     FirstName = request.FirstName,
                     LastName = request.LastName
                 }
             ).ConfigureAwait(false);
        }

    }
}
