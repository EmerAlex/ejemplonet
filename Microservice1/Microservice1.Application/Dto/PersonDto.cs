using System.Collections.Generic;

namespace Microservice1.Application.Person
{
    public class PersonDto
    {

        public PersonDto()
        {
            this.HomeAddress = new List<AddressDto>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public IEnumerable<AddressDto> HomeAddress { get; set; }
    }
}
