using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Microservice1.Application.Person
{
    public class CreatePersonCommand : IRequest
    {

        public CreatePersonCommand()
        {

        }

        public CreatePersonCommand(string firstName, string lastName, string email)
        {
            LastName = lastName;
            FirstName = firstName;
            Email = email;
        }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
