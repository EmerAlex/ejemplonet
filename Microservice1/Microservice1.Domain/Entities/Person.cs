using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microservice1.Domain.Entities
{
    public class Person : EntityBase<Guid>
    {
        const int DAYS_OF_THE_YEAR = 365;
        public Person()
        {
        }

        public Person(string firstName, string lastName,
            string email, IEnumerable<Address> address, DateTime date)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.HomeAddress = address;
            this.DateOfBirth = date;
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual IEnumerable<Address> HomeAddress { get; set; }
        public DateTime DateOfBirth { get; set; }


        public bool IsEmailValid()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(this.Email);
            return match.Success;
        }


        public int MiEdad()
        {
            return (DateTime.Now.Subtract(DateOfBirth)).Days / DAYS_OF_THE_YEAR;
        }



    }
}
