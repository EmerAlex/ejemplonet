using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice1.Domain.Entities
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
    }
}
