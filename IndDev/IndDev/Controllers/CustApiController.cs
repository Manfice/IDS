using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.Entity.Orders;

namespace IndDev.Controllers
{
    [Authorize(Roles = "A,C")]
    public class CustApiController : ApiController
    {
        private readonly ICustomer _customer;
        private int _userId;

        public CustApiController(ICustomer customer)
        {
            _customer = customer;
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _userId = int.Parse(HttpContext.Current.User.Identity.Name);
        }

        public IHttpActionResult GetCustomer()
        {
            var cust = new Customer();
            var custId = int.Parse(HttpContext.Current.User.Identity.Name);
            if (custId!=0)
            {
                cust = _customer.GetCustomerByUserId(custId);
            }
            return cust != null ? Ok(cust) : (IHttpActionResult) BadRequest("Customer was not finded");
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveAvatar()
        {
            var usr = _customer.GetCustomerByUserId(_userId);
            var request = HttpContext.Current.Request;
            if (!request.Files.AllKeys.Any()) return BadRequest("No file to save");
            var avatar = request.Files["avatar"];
            if (avatar == null) return BadRequest("Inncorrect file type");
            if (avatar.ContentLength > 524288) return BadRequest("Файл слишком большой.");
            var fileName = usr.Id + "_" + avatar.FileName;
            var fullPath = HttpContext.Current.Server.MapPath("~/Upload/CustomerAvatars/" + usr.Id + "_" + avatar.FileName);
            avatar.SaveAs(fullPath);
            var result = await _customer.SaveLogo(_userId, new CustomerLogo {AltText = usr.Title, FullPath = fullPath, Path = "/Upload/CustomerAvatars/"+fileName });

            return result!=null?Ok(result.Path):(IHttpActionResult)BadRequest("Inncorrect file type");
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _customer.GetOrdersAsync(_userId);
        } 

    }
}
