using MediatR;
using Microservice1.Application.Person;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microservice1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        private readonly IMediator _Mediator;

        public PersonController(IMediator mediator)
        {
            _Mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<Application.Person.PersonDto> GetById(Guid id)
        {
            return await _Mediator.Send(new Application.Person.PersonQuery(id));
        }



        [HttpPost]
        public async Task CreateUserAsync(CreatePersonCommandAsync person)
        {
            await _Mediator.Send(person);
        }

        [HttpPost("sync")]
        public async Task CreateUser(CreatePersonCommand person)
        {
            await _Mediator.Send(person);
        }





    }
}
