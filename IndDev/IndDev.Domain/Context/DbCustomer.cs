using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
using IndDev.Domain.Entity.Cart;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Orders;
using IndDev.Domain.ViewModels;

namespace IndDev.Domain.Context
{
    public class DbCustomer : ICustomer
    {
        private readonly DataContext _context = new DataContext();

        private int OrderNumber()
        {
            var nmb = 0;
            if (_context.Orders.Any())
            {
                nmb = _context.Orders.Max(order => order.Id);
            }
            nmb++;
            return nmb;
        }

        public Customer GetCustomerByUserId(int id)
        {
            var user = _context.Users.Find(id);
            return user.Customer;
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public Order MakeOrder(int userId, Cart cart, PreOrder preOrder)
        {
            var customer = _context.Users.Find(userId).Customer;
            var dType = _context.DeliveryTypes.Find(preOrder.DeliveryTypeCode);
            var deliv = new Delivery
            {
                DeliveryType = dType,
                Recipient = preOrder.Delivery.Recipient,
                To = preOrder.Delivery.To,
                Comment = preOrder.Delivery.Comment,
                DeliveryCost = cart.CalcTotalSumm() >= dType.FreeFrom ? 0 : dType.Cost
            };
            _context.Deliveries.Add(deliv);
            var ordNew = new OrderStatus
            {
                Moderated = false,
                Paid = false,
                UnderDelivery = false,
                DeliveryData = new DeliveryData() { DeliveryDate = DateTime.Now}
            };
            _context.OrderStatuses.Add(ordNew);
            var ord = new Order
            {
                Customer = customer,
                OrderDate = DateTime.Now,
                Number = OrderNumber(),
                Delivery = deliv,
                OrderStatus = ordNew
            };

            _context.Orders.Add(ord);


            foreach (var item in cart.CartItems)
            {
                var oLine = new OrderLine();
                var product = _context.Products.Find(item.Product.Id);
                var curr = _context.Currencies.Find(item.ActualPrice.Currency.Id);

                oLine.Currency = curr;
                oLine.CursOnDate = item.ActualPrice.Currency.Curs;
                oLine.Order = ord;
                oLine.Price = item.ActualPrice.ConvValue;
                oLine.Product = product;
                oLine.Quantity = item.Quantity;

                _context.OrderLines.Add(oLine);
            }
            _context.SaveChanges();
            return ord;
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders.Find(id);
        }
        public ValidEvent UpdateCustomer(User model)
        {
            var dbUser = _context.Users.Find(model.Id);
            dbUser.Name = model.Name;
            dbUser.Phone = model.Phone;
            dbUser.Region = model.Region;
            dbUser.Customer.Title = model.Customer.Title;
            dbUser.Customer.Adress = model.Customer.Adress;
            dbUser.Customer.Details.CompanyName = model.Customer.Details.CompanyName;
            dbUser.Customer.Details.UrAdress = model.Customer.Details.UrAdress;
            dbUser.Customer.Details.Inn = model.Customer.Details.Inn;
            dbUser.Customer.Details.Kpp = model.Customer.Details.Kpp;
            dbUser.Customer.Details.Ogrn = model.Customer.Details.Ogrn;
            _context.SaveChanges();
            return new ValidEvent {Code = dbUser.Id, Messge = "Ok"};
        }

        public List<DeliveryType> GetDeliveryTypes()
        {
            return _context.DeliveryTypes.ToList();
        }

        public IEnumerable<Order> GetOrders(int custId)
        {
            return _context.Orders.Where(order => order.Customer.Id==custId).ToList();
        }
    }
}