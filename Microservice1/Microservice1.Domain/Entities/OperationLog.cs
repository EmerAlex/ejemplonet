using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice1.Domain.Entities
{
    public class OperationLog : EntityBase<Guid>
    {
        public string LogContent { get; set; }
    }
}
