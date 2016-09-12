using System.Collections.Generic;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Abstract
{
    public interface ICrm
    {
        IEnumerable<Details> Company { get; } 
    }
}