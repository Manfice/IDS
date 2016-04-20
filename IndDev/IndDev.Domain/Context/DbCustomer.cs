using System;
using System.Threading.Tasks;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
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

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public ValidEvent UpdateCustomer(User model)
        {
            var dbUser = _context.Users.Find(model.Id);
            dbUser.Name = model.Name;
            dbUser.Phone = model.Phone;
            dbUser.Region = model.Region;
            dbUser.Customer.Title = model.Customer.Title;
            dbUser.Customer.Adress = model.Customer.Adress;
            _context.SaveChanges();
            return new ValidEvent {Code = dbUser.Id, Messge = "Ok"};
        }
    }
}