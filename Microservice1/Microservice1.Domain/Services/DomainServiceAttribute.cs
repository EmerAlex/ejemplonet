using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice1.Domain.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DomainServiceAttribute : Attribute
    {
    }
}
