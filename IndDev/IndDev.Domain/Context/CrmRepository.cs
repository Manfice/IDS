using System;
using System.Collections.Generic;
using System.Linq;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Context
{
    public class CrmRepository : ICrm
    {
        private readonly DataContext _context = new DataContext();

        public IEnumerable<Details> Company => _context.Detailses.Where(c=>!string.IsNullOrEmpty(c.CompanyName)).ToList();
    }
}