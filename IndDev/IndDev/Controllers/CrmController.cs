using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Customers;

namespace IndDev.Controllers
{
    [Authorize(Roles = "A,O,M")]
    public class CrmController : ApiController
    {
        private readonly ICrm _repo;
        private readonly IMailRepository _mail;

        public CrmController(ICrm repo, IMailRepository mail)
        {
            _repo = repo;
            _mail = mail;
        }

        [HttpGet]
        public IHttpActionResult GetCompanys()
        {
            return Ok(_repo.Company);
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
        public async Task<IHttpActionResult> UpdateCompany(Details currCompany)
        {
            var result = await _repo.UpdateCompany(currCompany);
            return result != null ? Ok(result) : (IHttpActionResult)BadRequest("Something going wrong...");

        }

        [HttpPost]
        public async Task<IHttpActionResult> SendKp(PersonContact contact)
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Views/Mails/letterkp.html");
            if (string.IsNullOrWhiteSpace(path)) return BadRequest("Не тела письма");
            var messageBody = System.IO.File.ReadAllText(path);
            var pers = await _repo.SendKpMarkAsync(contact);
            contact.Details = pers;
            var result = await _mail.SendKpAsynk(contact, messageBody);
            return result != null ? Ok(result) : (IHttpActionResult)BadRequest("Something going wrong...");
        }
    }
}
