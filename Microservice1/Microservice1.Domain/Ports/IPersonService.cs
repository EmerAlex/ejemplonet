using Microservice1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microservice1.Domain.Ports
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> FindPersonAsync(Expression<Func<Person, bool>> filter);
        Task<Person> SavePersonAsync(Person p);
    }

}
