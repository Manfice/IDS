using System;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Domain.Context
{
    public class DbCustomer : ICustomer
    {
        private readonly DataContext _context = new DataContext();
        public Customer GetCustomerByUserId(int id)
        {
            var user = _context.Users.Find(id);
            return user.Customer;
        }
    }
}