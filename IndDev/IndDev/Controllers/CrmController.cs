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

        public CrmController(ICrm repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IHttpActionResult GetCompanys()
        {
            return Ok(_repo.Company);
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
    }
}
