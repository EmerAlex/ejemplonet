using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice1.Domain.Entities
{
    public class Address : EntityBase<Guid>
    {

        public Address()
        {

        }
        public Address(string street, string number)
        {
            this.Street = street;
            this.Number = number;
        }

        public string Street { get; set; }
        public string Number { get; set; }

        public virtual Person Persona { get; set; }
        public Guid PersonId { get; set; }

    }
}
