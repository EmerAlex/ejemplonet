using MediatR;
using System.ComponentModel.DataAnnotations;


namespace Microservice1.Application.Person
{
    public class CreatePersonFromMessage : IRequest
    {
        public CreatePersonFromMessage(string firstName, string lastName, string email)
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
