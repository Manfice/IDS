using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Controllers;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Customers;
using IndDev.Domain.ViewModels;

namespace IndDev.Controllers
{
    [Authorize(Roles = "A,O,M")]
    public class CrmController : ApiController
    {
        private readonly ICrm _repo;
        private readonly IMailRepository _mail;
        private int _userId;

        public CrmController(ICrm repo, IMailRepository mail)
        {
            _repo = repo;
            _mail = mail;
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            if (User.Identity.IsAuthenticated)
            {
                _userId = int.Parse(User.Identity.Name);
            }
        }

        [HttpGet]
        public IHttpActionResult GetCompanys()
        {
            var result = _repo.GetCompanysByUser(_userId);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetCompany(int id)
        {
            return Ok(_repo.GetCompanyDetails(id));
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCompany(int id)
        {
            var result = await _repo.DeleteCompanyAsync(id);
            return result != null ? Ok(result) : (IHttpActionResult)BadRequest("Компания не найдена");
        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePhone(int id)
        {
            var result = await _repo.DeletePhoneAsync(id);
            return result != null ? Ok(result) : (IHttpActionResult)BadRequest("Телефон не найден");
        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePerson(int id)
        {
            var result = await _repo.DeleteContactAsync(id);
            return result != null ? Ok(result) : (IHttpActionResult)BadRequest("Контакт не найдена");
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdatePhone(Phone phone)
        {
            var result = await _repo.UpdatePhone(phone);
            return result != null ? Ok(result) : (IHttpActionResult)BadRequest("Контакт не найдена");
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateCompany(Details currCompany)
        {
            currCompany.Meneger = await _repo.GetUserById(_userId);
            var result = await _repo.UpdateCompany(currCompany);
            return result != null ? Ok(result) : (IHttpActionResult)BadRequest("Something going wrong...");

        }

        [HttpPost]
        public async Task<IHttpActionResult> SendKp(PersonContact contact)
        {
            var path = HostingEnvironment.MapPath("~/Views/Mails/letterkp.html");
            if (string.IsNullOrWhiteSpace(path)) return BadRequest("Не тела письма");
            var messageBody = File.ReadAllText(path);
            var pers = await _repo.SendKpMarkAsync(contact);
            contact.Details = pers;
            var result = await _mail.SendKpAsynk(contact, messageBody);
            return result != null ? Ok(result) : (IHttpActionResult)BadRequest("Something going wrong...");
        }
    }
}
